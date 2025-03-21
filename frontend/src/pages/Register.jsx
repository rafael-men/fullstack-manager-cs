import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { registerUser } from "../services/api";

const Register = () => {
  const [formData, setFormData] = useState({
    username: "",
    password: "",
    role: "",
    name: "",
    oab: "",
  });

  const [error, setError] = useState("");
  const [success, setSuccess] = useState(false);
  const navigate = useNavigate();

  const handleChange = (e) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
  };

  const handleRegister = async (e) => {
    e.preventDefault();
    setError("");
    setSuccess(false);


    if (formData.role === "Cliente" && formData.name !== formData.username) {
      setError("O nome deve ser igual ao usuário.");
      return;
    }

   
    const userPayload = {
      username: formData.username,
      password: formData.password,
      role: formData.role,
      nome: formData.role === "Cliente" ? formData.name : "", 
      oab: formData.role === "Procurador" ? formData.oab : "", 
    };

    try {
      await registerUser(userPayload);
      setSuccess(true);
      window.alert('Usuário Registrado com Sucesso')
      setTimeout(() => navigate("/login")); 
    } catch (err) {
      setError(err);
    }
  };

  return (
    <div className="flex min-h-screen items-center justify-center bg-gray-100">
      <div className="w-full max-w-md bg-white p-8 rounded-lg shadow-lg">
        <h2 className="text-2xl font-bold text-center mb-6">Registrar</h2>
        {error && <p className="text-red-500 text-sm">{error}</p>}
        {success && <p className="text-green-500 text-sm">Registro bem-sucedido! Redirecionando...</p>}

        <form onSubmit={handleRegister} className="space-y-4">
          <input
            type="text"
            name="username"
            placeholder="Usuário"
            value={formData.username}
            onChange={handleChange}
            className="w-full p-2 border rounded-md"
            required
          />
          <input
            type="password"
            name="password"
            placeholder="Senha"
            value={formData.password}
            onChange={handleChange}
            className="w-full p-2 border rounded-md"
            required
          />
          <select
            name="role"
            value={formData.role}
            onChange={handleChange}
            className="w-full p-2 border rounded-md"
            required
          >
            <option value="">Selecione um papel</option>
            <option value="Cliente">Cliente</option>
            <option value="Procurador">Procurador</option>
          </select>

          {formData.role === "Cliente" && (
            <input
              type="text"
              name="name"
              placeholder="Nome"
              value={formData.name}
              onChange={handleChange}
              className="w-full p-2 border rounded-md"
              required
            />
          )}

          {formData.role === "Procurador" && (
            <input
              type="text"
              name="oab"
              placeholder="Número OAB"
              value={formData.oab}
              onChange={handleChange}
              className="w-full p-2 border rounded-md"
              required
            />
          )}

          <button type="submit" className="w-full bg-blue-900 text-white p-2 rounded-md">
            Cadastrar
          </button>
        </form>

        <p className="mt-4 text-center">
          Já tem uma conta?{" "}
          <button onClick={() => navigate("/login")} className="text-blue-600 no-underline">
            Faça login
          </button>
        </p>
      </div>
    </div>
  );
};

export default Register;
