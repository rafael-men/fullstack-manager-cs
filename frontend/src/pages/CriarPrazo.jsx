import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import axios from "axios";
import jwtDecode from "jwt-decode";

const CriarPrazo = () => {
  const navigate = useNavigate();
  const [formData, setFormData] = useState({
    tipo: "",
    dataVencimento: new Date().toISOString().slice(0, 10),
    status: "",
  });

  useEffect(() => {
    const token = localStorage.getItem("token");

    if (!token) {
      alert("Sessão expirada. Faça login novamente.");
      navigate("/");
      return;
    }

    try {
      const decodedToken = jwtDecode(token);
      const currentTime = Math.floor(Date.now() / 1000);

      if (decodedToken.exp < currentTime) {
        alert("Sessão expirada. Faça login novamente.");
        localStorage.removeItem("token");
        navigate("/");
      }
    } catch (error) {
      console.error("Erro ao decodificar o token:", error);
      localStorage.removeItem("token");
      navigate("/");
    }
  }, [navigate]);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData({ ...formData, [name]: value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      const token = localStorage.getItem("token");
      const config = {
        headers: { Authorization: `Bearer ${token}`, "Content-Type": "application/json" },
      };

      const requestData = {
        ...formData,
        tipo: parseInt(formData.tipo, 10),
        status: parseInt(formData.status, 10),
      };

      await axios.post("http://localhost:5250/pge/prazos/novo", requestData, config);
      alert("Prazo criado com sucesso!");
      navigate("/procurador-home");
    } catch (error) {
      console.error("Erro ao criar prazo:", error);
      alert("Erro ao criar prazo. Verifique os dados.");
    }
  };

  return (
    <div className="flex justify-center items-center min-h-screen bg-gray-100 p-4">
      <div className="w-full max-w-lg bg-white shadow-lg rounded-lg p-6">
        <h2 className="text-2xl font-bold text-gray-700 text-center mb-6">Criar Prazo</h2>
        <form onSubmit={handleSubmit} className="space-y-4">

          <select name="tipo" onChange={handleChange} required className="w-full px-3 py-2 border rounded-md">
            <option value="">Selecione um Tipo</option>
            <option value="0">Recurso</option>
            <option value="1">Contestação</option>
            <option value="2">Manifestação</option>
          </select>

         
          <input
            type="date"
            name="dataVencimento"
            value={formData.dataVencimento}
            onChange={handleChange}
            required
            className="w-full px-3 py-2 border border-gray-300 rounded-md focus:ring-2 focus:ring-blue-500"
          />

          <select name="status" onChange={handleChange} required className="w-full px-3 py-2 border rounded-md">
            <option value="">Selecione um Status</option>
            <option value="0">Em Andamento</option>
            <option value="1">Vencido</option>
            <option value="2">Concluído</option>
          </select>

  
          <div className="flex flex-col sm:flex-row gap-2">
            <button type="submit" className="w-full sm:w-1/2 bg-blue-500 text-white py-2 rounded-md hover:bg-blue-600">
              Criar Prazo
            </button>
            <button
              type="button"
              onClick={() => navigate(-1)}
              className="w-full sm:w-1/2 bg-gray-300 text-gray-700 py-2 rounded-md hover:bg-gray-400"
            >
              Voltar
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};

export default CriarPrazo;
