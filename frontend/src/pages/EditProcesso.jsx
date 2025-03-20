import React, { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import axios from "axios";
import jwtDecode from "jwt-decode";

const EditarProcesso = () => {
  const navigate = useNavigate();
  const { numero } = useParams();
  const [formData, setFormData] = useState({
    numero: "",
    orgao: "",
    assunto: "",
    status: "",
    procuradorId: "",
    clientesIds: [],
    prazoId: "",
    documentoId: "",
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
      if (decodedToken.exp < Math.floor(Date.now() / 1000)) {
        alert("Sessão expirada. Faça login novamente.");
        localStorage.removeItem("token");
        navigate("/");
      }
    } catch (error) {
      console.error("Erro ao decodificar o token:", error);
      localStorage.removeItem("token");
      navigate("/");
    }

    axios
      .get(`http://localhost:5250/pge/processos/${numero}`, {
        headers: { Authorization: `Bearer ${token}` },
      })
      .then((response) => setFormData(response.data))
      .catch(() => alert("Erro ao buscar o processo."));
  }, [navigate, numero]);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData({ ...formData, [name]: value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    if (!formData.clientesIds.length) {
      alert("O processo deve ter pelo menos um cliente associado.");
      return;
    }
    if (!formData.procuradorId) {
      alert("O processo deve possuir um procurador.");
      return;
    }
    if (!formData.documentoId) {
      alert("O processo deve possuir um documento.");
      return;
    }
    if (!formData.prazoId) {
      alert("O processo deve possuir um prazo.");
      return;
    }
    if (![0, 1, 2, 3, 4].includes(parseInt(formData.orgao))) {
      alert("O órgão responsável deve ter um valor entre 0 e 4.");
      return;
    }
    if (![0, 1, 2, 3].includes(parseInt(formData.status))) {
      alert("O status deve ter um valor entre 0 e 3.");
      return;
    }

    try {
      const token = localStorage.getItem("token");
      await axios.put(`http://localhost:5250/pge/processos/atualizar/${numero}`, formData, {
        headers: { Authorization: `Bearer ${token}`, "Content-Type": "application/json" },
      });
      alert("Processo atualizado com sucesso!");
      navigate("/procurador-home");
    } catch (error) {
      console.error("Erro ao atualizar processo:", error);
      alert("Erro ao atualizar processo. Verifique os dados.");
    }
  };

  return (
    <div className="flex justify-center items-center min-h-screen bg-gray-100 p-4">
      <div className="w-full max-w-lg bg-white shadow-lg rounded-lg p-6">
        <h2 className="text-2xl font-bold text-gray-700 text-center mb-6">Editar Processo</h2>
        <form onSubmit={handleSubmit} className="space-y-4">
          <input type="text" name="assunto" value={formData.assunto} onChange={handleChange} required className="w-full px-3 py-2 border rounded-md" placeholder="Assunto" />
          <input type="number" name="procuradorId" value={formData.procuradorId} onChange={handleChange} required className="w-full px-3 py-2 border rounded-md" placeholder="ID do Procurador" />
          <input type="number" name="prazoId" value={formData.prazoId} onChange={handleChange} required className="w-full px-3 py-2 border rounded-md" placeholder="ID do Prazo" />
          <input type="number" name="documentoId" value={formData.documentoId} onChange={handleChange} required className="w-full px-3 py-2 border rounded-md" placeholder="ID do Documento" />
          <select name="orgao" onChange={handleChange} value={formData.orgao} required className="w-full px-3 py-2 border rounded-md">
            <option value="">Órgão Responsável</option>
            <option value="0">Órgão 0</option>
            <option value="1">Órgão 1</option>
            <option value="2">Órgão 2</option>
            <option value="3">Órgão 3</option>
            <option value="4">Órgão 4</option>
          </select>
          <select name="status" onChange={handleChange} value={formData.status} required className="w-full px-3 py-2 border rounded-md">
            <option value="">Status</option>
            <option value="0">Em Andamento</option>
            <option value="1">Vencido</option>
            <option value="2">Concluído</option>
            <option value="3">Arquivado</option>
          </select>
          <input type="text" name="clientesIds" value={formData.clientesIds} onChange={(e) => setFormData({ ...formData, clientesIds: e.target.value.split(",").map(id => id.trim()) })} required className="w-full px-3 py-2 border rounded-md" placeholder="IDs dos Clientes (separados por vírgula)" />
          <div className="flex flex-col sm:flex-row gap-2">
            <button type="submit" className="w-full sm:w-1/2 bg-blue-500 text-white py-2 rounded-md hover:bg-blue-600">Salvar Alterações</button>
            <button type="button" onClick={() => navigate(-1)} className="w-full sm:w-1/2 bg-gray-300 text-gray-700 py-2 rounded-md hover:bg-gray-400">Voltar</button>
          </div>
        </form>
      </div>
    </div>
  );
};

export default EditarProcesso;
