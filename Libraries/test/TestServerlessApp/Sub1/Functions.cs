using Amazon.Lambda.Annotations;
using Amazon.Lambda.Core;

namespace TestServerlessApp.Sub1
{
    public class Functions
    {
        [LambdaFunction(Name = "ToUpper", PackageType = LambdaPackageType.Image,
            Role = "arn:aws:iam::76803399999:role/service-role/Abc-role")]
        public string ToUpper(string text)
        {
            return text.ToUpper();
        }
    }
}
