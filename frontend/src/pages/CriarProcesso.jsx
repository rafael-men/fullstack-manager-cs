import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import axios from "axios";
import jwtDecode from "jwt-decode";

const CriarProcesso = () => {
  const navigate = useNavigate();
  const [formData, setFormData] = useState({
    numero: "",
    orgao: "",
    assunto: "",
    status: "",
    procuradorId: "",
    clientesIds: [],
    documentoId: "",
    prazoId: "",
  });

  const [procuradores, setProcuradores] = useState([]);
  const [clientes, setClientes] = useState([]);
  const [documentos, setDocumentos] = useState([]);
  const [prazos, setPrazos] = useState([]);

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

  useEffect(() => {
    const fetchData = async () => {
      try {
        const token = localStorage.getItem("token");
        const config = { headers: { Authorization: `Bearer ${token}` } };

        const [procuradoresRes, clientesRes, documentosRes, prazosRes] =
          await Promise.all([
            axios.get("http://localhost:5250/pge/procuradores", config),
            axios.get("http://localhost:5250/pge/clientes", config),
            axios.get("http://localhost:5250/pge/documentos", config),
            axios.get("http://localhost:5250/pge/prazos", config),
          ]);

      
        setProcuradores(Array.isArray(procuradoresRes.data) ? procuradoresRes.data : []);
        setClientes(Array.isArray(clientesRes.data) ? clientesRes.data : []);
        setDocumentos(Array.isArray(documentosRes.data) ? documentosRes.data : []);
        setPrazos(Array.isArray(prazosRes.data) ? prazosRes.data : []);
      } catch (error) {
        console.error("Erro ao buscar dados:", error);
        setProcuradores([]);
        setClientes([]);
        setDocumentos([]);
        setPrazos([]);
      }
    };

    fetchData();
  }, []);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData({ ...formData, [name]: value });
  };

  const handleMultiSelect = (e) => {
    const selectedIds = Array.from(e.target.selectedOptions, (option) => option.value);
    setFormData({ ...formData, clientesIds: selectedIds });
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
        orgao: parseInt(formData.orgao, 10),
        status: parseInt(formData.status, 10),
        procuradorId: formData.procuradorId ? parseInt(formData.procuradorId, 10) : null,
        documentoId: formData.documentoId ? parseInt(formData.documentoId, 10) : null,
        prazoId: formData.prazoId ? parseInt(formData.prazoId, 10) : null,
        clientesIds: formData.clientesIds.map((id) => parseInt(id, 10)),
      };

      await axios.post("http://localhost:5250/pge/processos/novo", requestData, config);
      alert("Processo criado com sucesso!");
      navigate("/procurador-home");
    } catch (error) {
      console.error("Erro ao criar processo:", error);
      alert("Erro ao criar processo. Verifique os dados.");
    }
  };

  return (
    <div className="flex justify-center items-center min-h-screen bg-gray-100">
      <div className="w-full max-w-3xl bg-white shadow-lg rounded-lg p-6">
        <h2 className="text-2xl font-bold text-gray-700 text-center mb-6">Criar Processo</h2>
        <form onSubmit={handleSubmit} className="space-y-4">
          <input
            type="text"
            name="numero"
            placeholder="Número do Processo"
            onChange={handleChange}
            required
            className="w-full px-3 py-2 border border-gray-300 rounded-md focus:ring-2 focus:ring-blue-500"
          />

          <select name="orgao" onChange={handleChange} required className="w-full px-3 py-2 border rounded-md">
            <option value="">Selecione um Órgão</option>
            <option value="0">Tribunal de Justiça</option>
            <option value="1">Juizado Especial</option>
            <option value="2">Vara Cível</option>
            <option value="3">Vara Criminal</option>
            <option value="4">Supremo Tribunal</option>
          </select>

          <input
            type="text"
            name="assunto"
            placeholder="Assunto"
            onChange={handleChange}
            required
            className="w-full px-3 py-2 border border-gray-300 rounded-md"
          />

          <select name="status" onChange={handleChange} required className="w-full px-3 py-2 border rounded-md">
            <option value="">Selecione um Status</option>
            <option value="0">Andamento</option>
            <option value="1">Concluído</option>
            <option value="2">Arquivado</option>
            <option value="3">Suspenso</option>
          </select>

          <select name="procuradorId" onChange={handleChange} required className="w-full px-3 py-2 border rounded-md">
            <option value="">Selecione um Procurador</option>
            {Array.isArray(procuradores) && procuradores.map((p) => (
              <option key={p.id} value={p.id}>
                OAB: {p.oab}
              </option>
            ))}
          </select>

          <select
            name="clientesIds"
            multiple
            onChange={handleMultiSelect}
            required
            className="w-full px-3 py-2 border rounded-md"
          >
            {Array.isArray(clientes) && clientes.map((c) => (
              <option key={c.id} value={c.id}>
                {c.nome}
              </option>
            ))}
          </select>

          <select name="documentoId" onChange={handleChange} required className="w-full px-3 py-2 border rounded-md">
            <option value="">Selecione um Documento</option>
            {Array.isArray(documentos) && documentos.map((d) => (
              <option key={d.id} value={d.id}>
                {d.nome}
              </option>
            ))}
          </select>

          <select name="prazoId" onChange={handleChange} required className="w-full px-3 py-2 border rounded-md">
            <option value="">Selecione um Prazo</option>
            {Array.isArray(prazos) && prazos.map((pr) => (
              <option key={pr.id} value={pr.id}>
                {pr.id}, tipo: {pr.tipo}
              </option>
            ))}
          </select>

          <div className="flex justify-between">
            <button type="submit" className="w-1/2 bg-blue-500 text-white py-2 rounded-md hover:bg-blue-600">
              Criar Processo
            </button>
            <button
              type="button"
              onClick={() => navigate(-1)}
              className="w-1/2 ml-2 bg-gray-300 text-gray-700 py-2 rounded-md hover:bg-gray-400"
            >
              Voltar
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};

export default CriarProcesso;
