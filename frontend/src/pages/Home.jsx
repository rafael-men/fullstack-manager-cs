import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';

const HomeCliente = () => {
  const [processos, setProcessos] = useState([]);
  const navigate = useNavigate();

  useEffect(() => {
    const fetchData = async () => {
      try {
        const token = localStorage.getItem('token');
        if (!token) {
          console.error("Token não encontrado!");
          return;
        }

        const response = await axios.get('http://localhost:5250/pge/processos', {
          headers: { Authorization: `Bearer ${token}` },
        });

        setProcessos(response.data);
      } catch (error) {
        console.error("Erro ao buscar dados", error);
      }
    };

    fetchData();
  }, []);

  const handleLogout = () => {
    localStorage.removeItem('token');
    navigate('/login');
  };

  return (
    <div className="container mx-auto p-4">
      <h1 className="text-3xl font-semibold mb-6">Processos</h1>

      <div className="mb-6 flex gap-4">
        <button onClick={() => navigate('/editar-cadastro-cliente')} className="px-4 py-2 bg-yellow-500 text-white rounded-md hover:bg-yellow-600">
          Alterar Credenciais
        </button>
        <button onClick={handleLogout} className="px-4 py-2 bg-red-500 text-white rounded-md hover:bg-red-600">
          Sair
        </button>
      </div>

      <div className="mb-6">
        <div className="overflow-x-auto bg-white shadow-md rounded-lg">
          <table className="min-w-full table-auto text-left text-sm">
            <thead>
              <tr className="bg-gray-200">
                <th className="px-4 py-2">Número</th>
                <th className="px-4 py-2">Órgão</th>
                <th className="px-4 py-2">Assunto</th>
                <th className="px-4 py-2">Status</th>
              </tr>
            </thead>
            <tbody>
              {processos.map((processo) => (
                <tr key={processo.numero} className="border-t hover:bg-gray-100">
                  <td className="px-4 py-2">{processo.numero}</td>
                  <td className="px-4 py-2">{processo.orgao}</td>
                  <td className="px-4 py-2">{processo.assunto}</td>
                  <td className="px-4 py-2">{processo.status}</td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      </div>
    </div>
  );
};

export default HomeCliente;
