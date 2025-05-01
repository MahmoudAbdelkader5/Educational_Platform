using Business_logic_layer.interfaces;
using Data_access_layer.model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Globalization;
using System.Text;
using System.Text.Json.Nodes;
using static NuGet.Packaging.PackagingConstants;

namespace Educational_Platform.Controllers
{
    public class CheckoutController : Controller
    {
        private string PaypalClientId { get; set; } = String.Empty;
        private string PaypalClientSecret { get; set; } = String.Empty;
        private string PaypalBaseUrl { get; set; } = String.Empty;
        private readonly IunitofWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;

        public CheckoutController(IConfiguration configuration, IunitofWork unitOfWork,
                 UserManager<ApplicationUser> userManager, IEmailSender emailSender)
        {
            PaypalClientId = configuration["PaypalSettings:ClientId"]!;
            PaypalClientSecret = configuration["PaypalSettings:Secret"]!;
            PaypalBaseUrl = configuration["PaypalSettings:Url"]!;
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _userManager = userManager;
            _emailSender = emailSender;
        }
        
        /*public async Task<string> Token()
        {
            string accessToken = await GetPaypalAccessToken();
            return accessToken;
        }*/
        private async Task<string> GetPaypalAccessToken()
        {
            string accessToken = string.Empty;
            string url = PaypalBaseUrl + "/v1/oauth2/token";
            using (var client = new HttpClient())
            {
                string credentials64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(PaypalClientId + ":" + PaypalClientSecret));

                client.DefaultRequestHeaders.Add("Authorization", "Basic " + credentials64);
                var requestMessage = new HttpRequestMessage(HttpMethod.Post, url);
                requestMessage.Content = new StringContent("grant_type=client_credentials", null, "application/x-www-form-urlencoded");

                var httpResponse = await client.SendAsync(requestMessage);
                if (httpResponse.IsSuccessStatusCode)
                {
                    var strResponse = await httpResponse.Content.ReadAsStringAsync();
                    var jsonResponse = JsonNode.Parse(strResponse);
                    if (jsonResponse != null)
                    {
                        accessToken = jsonResponse["access_token"]?.ToString() ?? "";
                    }
                }
            }
            return accessToken;
        }
        [HttpPost]
        public async Task<JsonResult> CreateOrder([FromBody] JsonObject data)
        {
            var totalAmount = data?["amount"]?.ToString();
            if (totalAmount == null)
            {
                return new JsonResult(new { Id = "" });
            }
            JsonObject createOrderRequest = new JsonObject();
            createOrderRequest.Add("intent", "CAPTURE");
            JsonObject amount = new JsonObject();
            amount.Add("currency_code", "USD");
            amount.Add("value", totalAmount);

            JsonObject purchaseUnit1 = new JsonObject();
            purchaseUnit1.Add("amount", amount);

            JsonArray purchaseUnits = new JsonArray();
            purchaseUnits.Add(purchaseUnit1);

            createOrderRequest.Add("purchase_units", purchaseUnits);
            string accessToken = await GetPaypalAccessToken();
            string url = PaypalBaseUrl + "/v2/checkout/orders";

            using (var client = new HttpClient())
            {
                // string credentials64=Convert.ToBase64String(Encoding.UTF8.GetBytes(PaypalClientId + ":" + PaypalClientSecret));

                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
                var requestMessage = new HttpRequestMessage(HttpMethod.Post, url);
                requestMessage.Content = new StringContent(createOrderRequest.ToString(), Encoding.UTF8, "application/json");

                var httpResponse = await client.SendAsync(requestMessage);
                if (httpResponse.IsSuccessStatusCode)
                {
                    var strResponse = await httpResponse.Content.ReadAsStringAsync();
                    var jsonResponse = JsonNode.Parse(strResponse);
                    if (jsonResponse != null)
                    {
                        string paypalOrderId = jsonResponse["id"]?.ToString() ?? "";
                        return new JsonResult(new { Id = paypalOrderId });
                    }
                }
            }

            return new JsonResult(new { Id = "" });
        }
        [HttpPost]
        public async Task<JsonResult> CompleteOrder([FromBody] JsonObject data)
        {
            var orderId = data["orderID"]?.ToString();
            if (orderId == null)
            {
                return new JsonResult("error");
            }
            string accessToken = await GetPaypalAccessToken();
            string url = PaypalBaseUrl + "/v2/checkout/orders/" + orderId + "/capture";
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
                var requestMessage = new HttpRequestMessage(HttpMethod.Post, url);

                // Fix: Use empty object instead of empty string
                requestMessage.Content = new StringContent("{}", Encoding.UTF8, "application/json");

                var httpResponse = await client.SendAsync(requestMessage);

                // Log error details if request fails
                if (!httpResponse.IsSuccessStatusCode)
                {
                    var errorResponse = await httpResponse.Content.ReadAsStringAsync();
                    Console.WriteLine($"PayPal API Error ({httpResponse.StatusCode}): {errorResponse}");
                    return new JsonResult("error");
                }

                // Success path remains the same
                var strResponse = await httpResponse.Content.ReadAsStringAsync();
                var jsonResponse = JsonNode.Parse(strResponse);
                if (jsonResponse != null)
                {
                    string paypalOrderStatus = jsonResponse["status"]?.ToString() ?? "";
                    if (paypalOrderStatus == "COMPLETED")
                    {
                        TempData["Amount"] = jsonResponse["purchase_units"]?[0]?["amount"]?["value"]?.ToString() ?? "0.00";
                        TempData["OrderId"] = orderId;
                        TempData.Keep("CourseId");
                        return new JsonResult("success");
                    }
                }
                return new JsonResult("error");
            }
        }

        public async Task<IActionResult> Index(string amount,int courseId)
        {

            if (decimal.TryParse(amount, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal parsedAmount))
            {
                ViewBag.Amount = parsedAmount;
                TempData["OriginalAmount"] = amount;
            }
            else
            {
                ViewBag.Amount = 0;
            }

            // Get course info
            var course = await _unitOfWork.Course.GetByIdAsync(courseId);
            if (course != null)
            {
                ViewBag.CourseTitle = course.Title;
                ViewBag.CourseDescription = course.Description;
                // Store courseId in TempData for later use
                TempData["CourseId"] = courseId;
            }

            ViewBag.PaypalClientId = PaypalClientId;
            return View();
        }
        public async Task<IActionResult> Success(string orderId)
        {
            ViewBag.OrderId = orderId ?? "N/A";
            ViewBag.Amount = TempData["Amount"]?.ToString() ?? "0.00";

            // Check if user is logged in
            if (User.Identity.IsAuthenticated)
            {
                try
                {
                    // Get current user
                    var user = await _userManager.GetUserAsync(User);
                    if (user != null)
                    {
                        // Get student record
                        var student = await _unitOfWork.Student.FindFirstAsync(s => s.UserId == user.Id);

                        // Get course ID
                        if (TempData["CourseId"] != null && int.TryParse(TempData["CourseId"].ToString(), out int courseId))
                        {
                            var course = await _unitOfWork.Course.GetByIdAsync(courseId);
                            string courseName = course?.Title ?? "Course"; // Default if null

                            // Store course details in ViewBag for the success page
                            ViewBag.CourseId = courseId;
                            ViewBag.CourseTitle = courseName;
                            // Check if already enrolled
                            var existingEnrollment = await _unitOfWork.student_CourseRepo.FindFirstAsync(sc => sc.StudentID == student.ID && sc.CourseID == courseId)
                                ;

                            if (existingEnrollment == null)
                            {
                                // Create new enrollment
                                var enrollment = new student_Course
                                {
                                    StudentID = student.ID,
                                    CourseID = courseId,
                                    EnrollmentDate = DateTime.UtcNow,
                                    PaymentStatus = "Completed"
                                };

                                await _unitOfWork.student_CourseRepo.AddAsync(enrollment);
                                await _unitOfWork.Save();
                                await SendPaymentConfirmationEmail(
                            user.Email,
                            user.FullName,
                            courseName,
                            ViewBag.Amount.ToString(),
                            orderId);
                                // Add success message
                                TempData["EnrollmentSuccess"] = "You have been successfully enrolled in this course!";
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Log error
                    Console.WriteLine($"Error enrolling student: {ex.Message}");
                }
            }

            return View();
        }
        private async Task SendPaymentConfirmationEmail(string email, string name, string courseName, string amount, string orderId)
        {
            string emailSubject = "Payment Confirmation - Your Course Purchase";

            string emailBody = $@"
        <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto; padding: 20px; border: 1px solid #eaeaea; border-radius: 5px;'>
            <div style='text-align: center; margin-bottom: 20px;'>
                <h1 style='color: #06BBCC;'>تم الدفع بنجاح!</h1>
            </div>
            
            <div style='margin-bottom: 20px;'>
                <p>عزيزي {name},</p>
                <p>شكرًا لشرائك. تم معالجة دفعتك بنجاح.</p>
                <p>لديك الآن وصول كامل إلى الدورة: <strong>{courseName}</strong></p>
            </div>
            
            <div style='background-color: #e9f9fa; padding: 15px; border-radius: 5px; margin-bottom: 20px;'>
                <h3 style='margin-top: 0;'>تفاصيل الشراء</h3>
                <p><strong>رقم الطلب:</strong> {orderId}</p>
                <p><strong>التاريخ:</strong> {DateTime.Now.ToString("MMM dd, yyyy")}</p>
            </div>
            
            <div style='margin-bottom: 20px;'>
                <p>يمكنك البدء في التعلم فورًا عن طريق تسجيل الدخول إلى حسابك والوصول إلى دوراتك.</p>
                <p>إذا كان لديك أي أسئلة أو تحتاج إلى مساعدة، فلا تتردد في الاتصال بفريق الدعم الخاص بنا.</p>
            </div>
            
            <div style='text-align: center; margin-top: 30px;'>
                <p style='color: #888; font-size: 12px;'>هذه رسالة تلقائية. يرجى عدم الرد على هذا البريد الإلكتروني.</p>
            </div>
        </div>
    ";

            await _emailSender.SendEmailAsync(email, emailSubject, emailBody);
        }

        public IActionResult Failure(string reason = "unknown", string errorMessage = null)
        {
            ViewBag.CourseId = TempData["CourseId"];
            ViewBag.Amount = TempData["OriginalAmount"]; 

            TempData.Keep("CourseId");
            TempData.Keep("OriginalAmount");
            ViewBag.Reason = reason;
            ViewBag.ErrorMessage = errorMessage ?? "We couldn't process your payment.";
            return View();
        }
    }
}
/*
 using Business_logic_layer.interfaces;
using Data_access_layer.model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text;
using System.Text.Json.Nodes;

namespace Educational_Platform.Controllers
{
    public class CheckoutController : Controller
    {
        private string PaypalClientId { get; set; } = string.Empty;
        private string PaypalClientSecret { get; set; } = string.Empty;
        private string PaypalBaseUrl { get; set; } = string.Empty;
        private readonly IunitofWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;

        public CheckoutController(IConfiguration configuration, IunitofWork unitOfWork,
                 UserManager<ApplicationUser> userManager, IEmailSender emailSender)
        {
            PaypalClientId = configuration["PaypalSettings:ClientId"]!;
            PaypalClientSecret = configuration["PaypalSettings:Secret"]!;
            PaypalBaseUrl = configuration["PaypalSettings:Url"]!;
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _userManager = userManager;
            _emailSender = emailSender;
        }

        private async Task<string> GetPaypalAccessToken()
        {
            string accessToken = string.Empty;
            string url = PaypalBaseUrl + "/v1/oauth2/token";
            using (var client = new HttpClient())
            {
                string credentials64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(PaypalClientId + ":" + PaypalClientSecret));

                client.DefaultRequestHeaders.Add("Authorization", "Basic " + credentials64);
                var requestMessage = new HttpRequestMessage(HttpMethod.Post, url);
                requestMessage.Content = new StringContent("grant_type=client_credentials", null, "application/x-www-form-urlencoded");

                var httpResponse = await client.SendAsync(requestMessage);
                if (httpResponse.IsSuccessStatusCode)
                {
                    var strResponse = await httpResponse.Content.ReadAsStringAsync();
                    var jsonResponse = JsonNode.Parse(strResponse);
                    if (jsonResponse != null)
                    {
                        accessToken = jsonResponse["access_token"]?.ToString() ?? "";
                    }
                }
            }
            return accessToken;
        }

        [HttpPost]
        public async Task<JsonResult> CreateOrder([FromBody] JsonObject data)
        {
            var totalAmount = data?["amount"]?.ToString();
            if (totalAmount == null)
            {
                return new JsonResult(new { Id = "" });
            }
            JsonObject createOrderRequest = new JsonObject();
            createOrderRequest.Add("intent", "CAPTURE");
            JsonObject amount = new JsonObject();
            amount.Add("currency_code", "USD");
            amount.Add("value", totalAmount);

            JsonObject purchaseUnit1 = new JsonObject();
            purchaseUnit1.Add("amount", amount);

            JsonArray purchaseUnits = new JsonArray();
            purchaseUnits.Add(purchaseUnit1);

            createOrderRequest.Add("purchase_units", purchaseUnits);
            string accessToken = await GetPaypalAccessToken();
            string url = PaypalBaseUrl + "/v2/checkout/orders";

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
                var requestMessage = new HttpRequestMessage(HttpMethod.Post, url);
                requestMessage.Content = new StringContent(createOrderRequest.ToString(), Encoding.UTF8, "application/json");

                var httpResponse = await client.SendAsync(requestMessage);
                if (httpResponse.IsSuccessStatusCode)
                {
                    var strResponse = await httpResponse.Content.ReadAsStringAsync();
                    var jsonResponse = JsonNode.Parse(strResponse);
                    if (jsonResponse != null)
                    {
                        string paypalOrderId = jsonResponse["id"]?.ToString() ?? "";
                        return new JsonResult(new { Id = paypalOrderId });
                    }
                }
            }

            return new JsonResult(new { Id = "" });
        }

        [HttpPost]
        public async Task<JsonResult> CompleteOrder([FromBody] JsonObject data)
        {
            var orderId = data["orderID"]?.ToString();
            if (orderId == null)
            {
                return new JsonResult("خطأ");
            }
            string accessToken = await GetPaypalAccessToken();
            string url = PaypalBaseUrl + "/v2/checkout/orders/" + orderId + "/capture";
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
                var requestMessage = new HttpRequestMessage(HttpMethod.Post, url);

                requestMessage.Content = new StringContent("{}", Encoding.UTF8, "application/json");

                var httpResponse = await client.SendAsync(requestMessage);

                if (!httpResponse.IsSuccessStatusCode)
                {
                    var errorResponse = await httpResponse.Content.ReadAsStringAsync();
                    Console.WriteLine($"خطأ في واجهة برمجة تطبيقات PayPal ({httpResponse.StatusCode}): {errorResponse}");
                    return new JsonResult("خطأ");
                }

                var strResponse = await httpResponse.Content.ReadAsStringAsync();
                var jsonResponse = JsonNode.Parse(strResponse);
                if (jsonResponse != null)
                {
                    string paypalOrderStatus = jsonResponse["status"]?.ToString() ?? "";
                    if (paypalOrderStatus == "COMPLETED")
                    {
                        TempData["Amount"] = jsonResponse["purchase_units"]?[0]?["amount"]?["value"]?.ToString() ?? "0.00";
                        TempData["OrderId"] = orderId;
                        TempData.Keep("CourseId");
                        return new JsonResult("نجاح");
                    }
                }
                return new JsonResult("خطأ");
            }
        }

        public async Task<IActionResult> Index(string amount, int courseId)
        {
            if (decimal.TryParse(amount, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal parsedAmount))
            {
                ViewBag.Amount = parsedAmount;
                TempData["OriginalAmount"] = amount;
            }
            else
            {
                ViewBag.Amount = 0;
            }

            var course = await _unitOfWork.Course.GetByIdAsync(courseId);
            if (course != null)
            {
                ViewBag.CourseTitle = course.Title;
                ViewBag.CourseDescription = course.Description;
                TempData["CourseId"] = courseId;
            }

            ViewBag.PaypalClientId = PaypalClientId;
            return View();
        }

        public async Task<IActionResult> Success(string orderId)
        {
            ViewBag.OrderId = orderId ?? "غير متوفر";
            ViewBag.Amount = TempData["Amount"]?.ToString() ?? "0.00";

            if (User.Identity.IsAuthenticated)
            {
                try
                {
                    var user = await _userManager.GetUserAsync(User);
                    if (user != null)
                    {
                        var student = await _unitOfWork.Student.FindFirstAsync(s => s.UserId == user.Id);

                        if (TempData["CourseId"] != null && int.TryParse(TempData["CourseId"].ToString(), out int courseId))
                        {
                            var course = await _unitOfWork.Course.GetByIdAsync(courseId);
                            string courseName = course?.Title ?? "الدورة التدريبية";

                            ViewBag.CourseId = courseId;
                            ViewBag.CourseTitle = courseName;

                            var existingEnrollment = await _unitOfWork.student_CourseRepo.FindFirstAsync(sc => sc.StudentID == student.ID && sc.CourseID == courseId);

                            if (existingEnrollment == null)
                            {
                                var enrollment = new student_Course
                                {
                                    StudentID = student.ID,
                                    CourseID = courseId,
                                    EnrollmentDate = DateTime.UtcNow,
                                    PaymentStatus = "مكتمل"
                                };

                                await _unitOfWork.student_CourseRepo.AddAsync(enrollment);
                                await _unitOfWork.Save();
                                await SendPaymentConfirmationEmail(
                                    user.Email,
                                    user.FullName,
                                    courseName,
                                    ViewBag.Amount.ToString(),
                                    orderId);
                                TempData["EnrollmentSuccess"] = "تم تسجيلك بنجاح في هذه الدورة!";
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"خطأ أثناء تسجيل الطالب: {ex.Message}");
                }
            }

            return View();
        }

        private async Task SendPaymentConfirmationEmail(string email, string name, string courseName, string amount, string orderId)
        {
            string emailSubject = "تأكيد الدفع - شراء الدورة التدريبية";

            string emailBody = $@"
        <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto; padding: 20px; border: 1px solid #eaeaea; border-radius: 5px;'>
            <div style='text-align: center; margin-bottom: 20px;'>
                <h1 style='color: #06BBCC;'>تم الدفع بنجاح!</h1>
            </div>
            
            <div style='margin-bottom: 20px;'>
                <p>عزيزي {name},</p>
                <p>شكرًا لشرائك. تم معالجة دفعتك بنجاح.</p>
                <p>لديك الآن وصول كامل إلى الدورة: <strong>{courseName}</strong></p>
            </div>
            
            <div style='background-color: #e9f9fa; padding: 15px; border-radius: 5px; margin-bottom: 20px;'>
                <h3 style='margin-top: 0;'>تفاصيل الشراء</h3>
                <p><strong>رقم الطلب:</strong> {orderId}</p>
                <p><strong>التاريخ:</strong> {DateTime.Now.ToString("MMM dd, yyyy")}</p>
            </div>
            
            <div style='margin-bottom: 20px;'>
                <p>يمكنك البدء في التعلم فورًا عن طريق تسجيل الدخول إلى حسابك والوصول إلى دوراتك.</p>
                <p>إذا كان لديك أي أسئلة أو تحتاج إلى مساعدة، فلا تتردد في الاتصال بفريق الدعم الخاص بنا.</p>
            </div>
            
            <div style='text-align: center; margin-top: 30px;'>
                <p style='color: #888; font-size: 12px;'>هذه رسالة تلقائية. يرجى عدم الرد على هذا البريد الإلكتروني.</p>
            </div>
        </div>
    ";

            await _emailSender.SendEmailAsync(email, emailSubject, emailBody);
        }

        public IActionResult Failure(string reason = "غير معروف", string errorMessage = null)
        {
            ViewBag.CourseId = TempData["CourseId"];
            ViewBag.Amount = TempData["OriginalAmount"];

            TempData.Keep("CourseId");
            TempData.Keep("OriginalAmount");
            ViewBag.Reason = reason;
            ViewBag.ErrorMessage = errorMessage ?? "لم نتمكن من معالجة دفعتك.";
            return View();
        }
    }
}

 */