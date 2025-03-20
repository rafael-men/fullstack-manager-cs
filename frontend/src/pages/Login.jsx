import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { loginUser } from "../services/api";
import  jwtDecode  from "jwt-decode";

const Login = () => {
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");
  const navigate = useNavigate();

  const handleLogin = async (e) => {
    e.preventDefault();
    setError("");

    try {
      const response = await loginUser({ username, password });

      
      const Token = String(response.token);
      

     
      if (!Token || Token.split('.').length !== 3) {
        setError("Token inválido ou mal formatado.");
        return;
      }

      
      localStorage.setItem("token", Token);

      
      const decodedToken = jwtDecode(Token);

  
      const role = decodedToken["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];

  
      if (role === "Procurador") {
        navigate("/procurador-home");
      } else {
        navigate("/home");
      }

      window.alert("Usuário Logado");

    } catch (err) {
      setError(err.message || "Erro ao realizar login.");
    }
  };

  return (
    <div className="flex min-h-screen items-center justify-center bg-gray-100">
      <div className="w-full max-w-md bg-white p-8 rounded-lg shadow-lg">
        <h2 className="text-2xl font-bold text-center mb-6">Login</h2>
        {error && <p className="text-red-500 text-sm">{error}</p>}
        <form onSubmit={handleLogin} className="space-y-4">
          <input
            type="text"
            placeholder="Usuário"
            value={username}
            onChange={(e) => setUsername(e.target.value)}
            className="w-full p-2 border rounded-md"
          />
          <input
            type="password"
            placeholder="Senha"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            className="w-full p-2 border rounded-md"
          />
          <button type="submit" className="w-full bg-blue-900 text-white p-2 rounded-md">
            Entrar
          </button>
        </form>
        <p className="mt-4 text-center">
          Ainda não tem uma conta?{" "}
          <button onClick={() => navigate("/register")} className="text-blue-500 no-underline">
            Registre-se aqui
          </button>
        </p>
      </div>
    </div>
  );
};

export default Login;
