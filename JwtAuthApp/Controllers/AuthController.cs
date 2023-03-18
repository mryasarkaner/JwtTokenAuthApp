using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace jwtauthapp.Controllers;
[ApiController]
[Route("[controller]")]


public class AuthController : ControllerBase{

    string myKey = "SecretKeyimBuralardaBiryerde";


    [HttpGet]
    public string Get(string userName, string password){

        // Bu claimler bize nesnelerin veritabanındaki bilgilerini veren payloadlardır mesela rolelr gibi,
        //veya benzersiz guidler gibi içerde taşımak istediğim tüm veriler yani

        var claims = new[]{
            new Claim(ClaimTypes.Name,userName),
            new Claim(JwtRegisteredClaimNames.Email, userName)
        };

       
        var securityKey=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(myKey)); // burada string keyimizi bitlere dönüştürüp hemde türkçe karakter içersin şeklinde şifremlemiz için uygun hale geliyor
        var credentials=new SigningCredentials(securityKey,SecurityAlgorithms.HmacSha256);


        var jwtSecurityToken = new JwtSecurityToken(
            issuer:"https://kaneryazilim.site",
            audience: "KanerAuth",
            claims: claims,
            expires: DateTime.Now.AddDays(15),//15 gün sonra bu token geçersiz kalıcak
            notBefore: DateTime.Now,//geçerliliği hemen başlasın
            signingCredentials:credentials
        );
        var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        return token;

    }
    [HttpGet("ValidateToken")]
    public bool ValidateToken(string token){
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(myKey));//yine sifremizi bitlere dönüştörüp decryption yapması için

        try{
            JwtSecurityTokenHandler handler = new();
            handler.ValidateToken(token, new TokenValidationParameters(){

                ValidateIssuerSigningKey= true,
                IssuerSigningKey = securityKey,
                ValidateLifetime=true,
                ValidateAudience= false,

            },out SecurityToken validatedToken);

            var jwtToken =(JwtSecurityToken)validatedToken;
            var claims= jwtToken.Claims.ToList();
            return true;


        }
        catch(System.Exception)
        {
            return false;

        }



    }
    
}
// jwt config tamamdır normalde bunu config dosyası ile gönderirim Mentorumm

