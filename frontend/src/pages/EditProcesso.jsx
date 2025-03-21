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
      const response = await axios.put(`http://localhost:5250/pge/processos/atualizar/${numero}`, formData, {
        headers: { Authorization: `Bearer ${token}`, "Content-Type": "application/json" },
      });
  
     
      if (response.data.error) {
        if (response.data.error === "procuradorId not found") {
          alert("Procurador não encontrado.");
        } else if (response.data.error === "documentoId not found") {
          alert("Documento não encontrado.");
        } else if (response.data.error === "prazoId not found") {
          alert("Prazo não encontrado.");
        } else if (response.data.error === "clientesIds not found") {
          alert("Clientes não encontrados.");
        } else {
          alert("Erro ao atualizar processo.");
        }
        return;
      }
  
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
        <form onSubmit={handleSubmit} className="grid gap-4">
          {[{ label: "Assunto", name: "assunto", type: "text" },
            { label: "Id do Procurador", name: "procuradorId", type: "number" },
            { label: "Id do Prazo", name: "prazoId", type: "number" },
            { label: "Id do Documento", name: "documentoId", type: "number" },
          ].map(({ label, name, type }) => (
            <div key={name} className="flex flex-col">
              <label className="text-gray-600 font-medium">{label}</label>
              <input type={type} name={name} value={formData[name]} onChange={handleChange} required className="w-full px-3 py-2 border rounded-md" placeholder={label} />
            </div>
          ))}
          
          <div className="flex flex-col">
            <label className="text-gray-600 font-medium">Órgão Responsável</label>
            <select name="orgao" onChange={handleChange} value={formData.orgao} required className="w-full px-3 py-2 border rounded-md">
              <option value="">Selecione um Órgão</option>
              <option value="0">Tribunal de Justiça</option>
              <option value="1">Juizado Especial</option>
              <option value="2">Vara Cível</option>
              <option value="3">Vara Criminal</option>
              <option value="4">Supremo Tribunal</option>
            </select>
          </div>

          <div className="flex flex-col">
            <label className="text-gray-600 font-medium">Status</label>
            <select name="status" onChange={handleChange} value={formData.status} required className="w-full px-3 py-2 border rounded-md">
              <option value="">Selecione um Status</option>
              <option value="0">Andamento</option>
              <option value="1">Concluído</option>
              <option value="2">Arquivado</option>
              <option value="3">Suspenso</option>
            </select>
          </div>

          <div className="flex flex-col">
            <label className="text-gray-600 font-medium">Clientes Associados (IDs)</label>
            <input type="text" name="clientesIds" value={formData.clientesIds} onChange={(e) => setFormData({ ...formData, clientesIds: e.target.value.split(",").map(id => id.trim()) })} required className="w-full px-3 py-2 border rounded-md" placeholder="IDs dos Clientes (separados por vírgula)" />
          </div>

          <div className="flex flex-col sm:flex-row gap-2 mt-4">
            <button type="submit" className="w-full sm:w-1/2 bg-blue-500 text-white py-2 rounded-md hover:bg-blue-600">Salvar</button>
            <button type="button" onClick={() => navigate(-1)} className="w-full sm:w-1/2 bg-gray-300 text-gray-700 py-2 rounded-md hover:bg-gray-400">Voltar</button>
          </div>
        </form>
      </div>
    </div>
  );
};

export default EditarProcesso;
