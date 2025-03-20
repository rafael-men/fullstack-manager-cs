import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';

const HomeProcurador = () => {
  const [processos, setProcessos] = useState([]);
  const [procuradores, setProcuradores] = useState([]);
  const [documentos, setDocumentos] = useState([]);
  const navigate = useNavigate();

  useEffect(() => {
    const fetchData = async () => {
      try {
        const token = localStorage.getItem('token');
        if (!token) {
          console.error("Token não encontrado!");
          return;
        }

        const [processosRes, procuradoresRes, documentosRes] = await Promise.all([
          axios.get('http://localhost:5250/pge/processos', {
            headers: { Authorization: `Bearer ${token}` },
          }),
          axios.get('http://localhost:5250/pge/procuradores', {
            headers: { Authorization: `Bearer ${token}` },
          }),
          axios.get('http://localhost:5250/pge/documentos', {
            headers: { Authorization: `Bearer ${token}` },
          }),
        ]);

        setProcessos(processosRes.data);
        setProcuradores(procuradoresRes.data);
        setDocumentos(documentosRes.data);
      } catch (error) {
        console.error("Erro ao buscar dados", error);
      }
    };

    fetchData();
  }, []);

  const handleEdit = (id) => {
    navigate(`/processos/editar/${id}`);
  };

  const handleDeleteDocumento = async (id) => {
    
    const documentoAssociado = processos.some((processo) => processo.documentoId === id);

    if (documentoAssociado) {
      alert("Este documento não pode ser excluído, pois está associado a um processo.");
      return;
    }

   
    const confirmarExclusao = window.confirm(`Deseja realmente excluir o documento?`);
    if (confirmarExclusao) {
      try {
        const token = localStorage.getItem('token');
        if (!token) {
          console.error("Token não encontrado!");
          return;
        }

        
        const response = await axios.delete(`http://localhost:5250/pge/documentos/deletar/${id}`, {
          headers: { Authorization: `Bearer ${token}` },
        });

        if (response.status === 200) {
          setDocumentos((prevDocumentos) => prevDocumentos.filter((documento) => documento.id !== id));
          alert("Documento excluído com sucesso");
        }
      } catch (error) {
        console.error("Erro ao excluir documento", error);
      }
    }
  };

  const handleDeleteProcesso = async (numero) => {
    const confirmarExclusao = window.confirm(`Deseja realmente excluir o processo ${numero}?`);
  
    if (confirmarExclusao) {
      try {
        const token = localStorage.getItem('token');
        if (!token) {
          console.error("Token não encontrado!");
          return;
        }
  
        const response = await axios.delete(`http://localhost:5250/pge/processos/deletar/${numero}`, {
          headers: { Authorization: `Bearer ${token}` },
        });
  
        if (response.status === 200) {
          setProcessos((prevProcessos) => prevProcessos.filter((processo) => processo.numero !== numero));
          alert("Processo excluído com sucesso!");
        }
      } catch (error) {
        console.error("Erro ao excluir processo", error);
      }
    }
  };
  

  return (
    <div className="container mx-auto p-4">
      <h1 className="text-3xl font-semibold mb-6">Todos os Processos, Procuradores e Documentos</h1>

      <div className="mb-6 flex gap-4">
        <button onClick={() => navigate('/processos/criar')} className="px-4 py-2 bg-blue-500 text-white rounded-md hover:bg-blue-600">
          Criar Processo
        </button>
        <button onClick={() => navigate('/documentos/criar')} className="px-4 py-2 bg-green-500 text-white rounded-md hover:bg-green-600">
          Criar Documento
        </button>
        <button onClick={() => navigate('/prazos/criar')} className="px-4 py-2 bg-yellow-500 text-white rounded-md hover:bg-yellow-600">
          Criar Prazo
        </button>
        <button onClick={() => navigate('/editar-cadastro')} className="px-4 py-2 bg-orange-700 text-white rounded-md hover:bg-orange-800">
          Alterar Senha
        </button>
        <button onClick={() => navigate('/')} className="px-4 py-2 bg-red-700 text-white rounded-md hover:bg-red-800">
          Sair
        </button>
      </div>

      <div className="mb-6">
  <h2 className="text-xl font-semibold mb-4">Processos</h2>
  <div className="overflow-x-auto bg-white shadow-md rounded-lg">
    <table className="min-w-full table-auto text-left text-sm">
      <thead>
        <tr className="bg-gray-200">
          <th className="px-4 py-2">Número</th>
          <th className="px-4 py-2">Órgão</th>
          <th className="px-4 py-2">Assunto</th>
          <th className="px-4 py-2">Status</th>
          <th className="px-4 py-2">Clientes</th>
          <th className="px-4 py-2">Procurador</th>
          <th className="px-4 py-2">Prazo</th>
          <th className="px-4 py-2">Documento</th>
          <th className="px-4 py-2">Ações</th>
        </tr>
      </thead>
      <tbody>
        {processos.map((processo) => (
          <tr key={processo.numero} className="border-t hover:bg-gray-100">
            <td className="px-4 py-2">{processo.numero}</td>
            <td className="px-4 py-2">{processo.orgao}</td>
            <td className="px-4 py-2">{processo.assunto}</td>
            <td className="px-4 py-2">{processo.status}</td>
            <td className="px-4 py-2">{processo.clientesIds.join(', ')}</td>
            <td className="px-4 py-2">{processo.procuradorId}</td>
            <td className="px-4 py-2">{processo.prazoId}</td>
            <td className="px-4 py-2">{processo.documentoId}</td>
            <td className="px-4 py-2 flex gap-2">
              <button onClick={() => handleEdit(processo.numero)} className="px-4 py-2 bg-yellow-500 text-white rounded-md hover:bg-yellow-600">
                Editar
              </button>
              <button onClick={() => handleDeleteProcesso(processo.numero)} className="px-4 py-2 bg-red-500 text-white rounded-md hover:bg-red-600">
                Excluir
              </button>
            </td>
          </tr>
        ))}
      </tbody>
    </table>
  </div>
</div>


    
      <div className="mb-6">
        <h2 className="text-xl font-semibold mb-4">Procuradores</h2>
        <div className="overflow-x-auto bg-white shadow-md rounded-lg">
          <table className="min-w-full table-auto text-left text-sm">
            <thead>
              <tr className="bg-gray-200">
                <th className="px-4 py-2">ID</th>
                <th className="px-4 py-2">OAB</th>
                <th className="px-4 py-2">Processos</th>
              </tr>
            </thead>
            <tbody>
              {procuradores.map((procurador) => (
                <tr key={procurador.id} className="border-t hover:bg-gray-100">
                  <td className="px-4 py-2">{procurador.id}</td>
                  <td className="px-4 py-2">{procurador.oab}</td>
                  <td className="px-4 py-2">{procurador.processosIds.join(', ')}</td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      </div>

      
      <div className="mb-6">
        <h2 className="text-xl font-semibold mb-4">Documentos</h2>
        <div className="overflow-x-auto bg-white shadow-md rounded-lg">
          <table className="min-w-full table-auto text-left text-sm">
            <thead>
              <tr className="bg-gray-200">
                <th className="px-4 py-2">ID</th>
                <th className="px-4 py-2">Nome</th>
                <th className="px-4 py-2">Tipo</th>
                <th className="px-4 py-2">Data de Cadastro</th>
                <th className="px-4 py-2">Ações</th>
              </tr>
            </thead>
            <tbody>
              {documentos.map((documento) => (
                <tr key={documento.id} className="border-t hover:bg-gray-100">
                  <td className="px-4 py-2">{documento.id}</td>
                  <td className="px-4 py-2">{documento.nome}</td>
                  <td className="px-4 py-2">{documento.tipo}</td>
                  <td className="px-4 py-2">{new Date(documento.dataCadastro).toLocaleString()}</td>
                  <td className="px-4 py-2 flex gap-2">
                    <button onClick={() => handleDeleteDocumento(documento.id)} className="px-4 py-2 bg-red-500 text-white rounded-md hover:bg-red-600">
                      Excluir
                    </button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      </div>
    </div>
  );
};

export default HomeProcurador;
