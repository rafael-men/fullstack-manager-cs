using System.Linq;
using System;
using BCrypt.Net;
using main.Data;
using main.Models;
using static main.Controllers.UserController;

public class UserService
{
    private readonly AppDbContext _context;

    public UserService(AppDbContext context)
    {
        _context = context;
    }

    public User Authenticate(string username, string password)
    {

        var user = _context.Users.SingleOrDefault(u => u.Username == username);

        if (user != null)
        {
            Console.WriteLine($"Verificando usuário {user.Username}. Hash da senha: {user.PasswordHash}");
            Console.WriteLine($"Senha fornecida: {password}");

          
            if (BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                return user;
            }
        }

        return null; 
    }


    public void AddUser(UserRegisterRequest userRegisterRequest)
    {
       
        if (_context.Users.Any(u => u.Username == userRegisterRequest.Username))
        {
            throw new Exception("Usuário já existe");
        }

        
        var user = new User
        {
            Username = userRegisterRequest.Username,
            Password = userRegisterRequest.Password,
            Role = userRegisterRequest.Role
        };

        
        if (userRegisterRequest.Role == "Procurador")
        {
            
            if (string.IsNullOrEmpty(userRegisterRequest.OAB))
            {
                throw new Exception("OAB é obrigatória para Procurador.");
            }

            if (!string.IsNullOrEmpty(userRegisterRequest.Nome))
            {
                throw new Exception("Nome não pode ser fornecido para Procurador.");
            }


            var procurador = new Procurador
            {
                OAB = userRegisterRequest.OAB,
                ProcessosIds = new List<int>()  
            };

            
            _context.Procuradores.Add(procurador);
            _context.SaveChanges();  

            
        }

        else if (userRegisterRequest.Role == "Cliente")
        {

            if (!string.IsNullOrEmpty(userRegisterRequest.OAB))
            {
                throw new Exception("OAB não pode ser fornecido para Cliente.");
            }

            if (string.IsNullOrEmpty(userRegisterRequest.Nome))
            {
                throw new Exception("Nome é obrigatório para Cliente.");
            }

            var cliente = new Cliente
            {
                Nome = userRegisterRequest.Nome
            };

           
            _context.Clientes.Add(cliente);
            _context.SaveChanges();  
        }


        user.SetPassword(userRegisterRequest.Password);

       
        _context.Users.Add(user);
        _context.SaveChanges();
    }



    public void UpdateCredentials(string username, string oldPassword, string newPassword)
    {
        var user = _context.Users.SingleOrDefault(u => u.Username == username);

        if (user == null)
        {
            throw new Exception("Usuário não encontrado");
        }

        
        if (!BCrypt.Net.BCrypt.Verify(oldPassword, user.PasswordHash))
        {
            throw new Exception("Senha antiga incorreta");
        }

   
        user.SetPassword(newPassword); 

        _context.Users.Update(user);
        _context.SaveChanges();
    }
}
