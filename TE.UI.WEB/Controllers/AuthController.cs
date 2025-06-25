using Microsoft.AspNetCore.Mvc;
using TE.Core.Domain;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using System.Linq;
using TE.Core.Services;
using Microsoft.AspNetCore.Identity;
using TE.Data;

namespace TE.UI.WEB.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly TokenService _tokenService;
        
        private readonly TEContext _context;


        public AuthController(TokenService tokenService, TEContext context)
        {
            _tokenService = tokenService;
           
            _context = context;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            // 1. Vérifier si l'email et le mot de passe sont valides (ici on simule une validation basique)
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest("Email et mot de passe sont requis.");
            }
            var user = _context.Users.FirstOrDefault(u => u.Email == request.Email);
            if (user == null)
            {
                return Unauthorized("Utilisateur introuvable.");
            }
            // unhash pass 
            var hasher = new PasswordHasher<User>();
            var result = hasher.VerifyHashedPassword(user, user.Password, request.Password);

            // 2. Simuler une vérification avec un utilisateur fictif
            if (result == PasswordVerificationResult.Failed)
            {
                return Unauthorized("Mot de passe incorrect.");

               
            }
            var token = _tokenService.GenerateJwtToken(user);

            return Ok(new { token });
        }
        [HttpPost("signup/patient")]
        public IActionResult SignupPatient([FromBody] Patient request)
        {
         

            // Hasher le mot de passe
            var hasher = new PasswordHasher<User>();
            request.Password = hasher.HashPassword(null, request.Password);

            // Sauvegarder dans la base
            _context.Add(request); // EF comprend que c'est un Patient
            FicheMedical fiche = new FicheMedical();
            fiche.patient = request;
            fiche.sexe = request.sexe;

            _context.Add(fiche);
            _context.SaveChanges();

            // Générer le token JWT
            var token = _tokenService.GenerateJwtToken(request);

            return Ok(new { token });
        }
        [HttpPost("signup/medecin")]
        public IActionResult SignupMedecin([FromBody] Medecin request)
        {


            // Hasher le mot de passe
            var hasher = new PasswordHasher<User>();
            request.Password = hasher.HashPassword(null, request.Password);

            // Sauvegarder dans la base
            _context.Add(request); // EF comprend que c'est un Medecin
            _context.SaveChanges();

            // Générer le token JWT
            var token = _tokenService.GenerateJwtToken(request);

            return Ok(new { token });
        }


    }
      


    // Classe pour représenter les données d'authentification (login)
    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }


}
