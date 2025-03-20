import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';
import jwtDecode from 'jwt-decode';

const EditarCredenciais = () => {
  const [username, setUsername] = useState('');
  const [oldPassword, setOldPassword] = useState('');
  const [newPassword, setNewPassword] = useState('');
  const navigate = useNavigate();

  useEffect(() => {
    const token = localStorage.getItem('token');

    if (!token) {
      alert('Sessão expirada. Faça login novamente.');
      navigate('/');
      return;
    }

    try {
      const decodedToken = jwtDecode(token);
      const currentTime = Math.floor(Date.now() / 1000);
      
      if (decodedToken.exp < currentTime) {
        alert('Sessão expirada. Faça login novamente.');
        localStorage.removeItem('token');
        navigate('/');
      }
    } catch (error) {
      console.error('Erro ao decodificar o token:', error);
      localStorage.removeItem('token');
      navigate('/');
    }
  }, [navigate]);

  const handleUpdate = async () => {
    try {
      const token = localStorage.getItem('token');

      if (!token) {
        console.error('Usuário não autenticado.');
        alert('Sessão expirada. Faça login novamente.');
        navigate('/');
        return;
      }

      const config = {
        headers: {
          Authorization: `Bearer ${token}`,
          'Content-Type': 'application/json',
        },
      };

      const data = { username, oldPassword, newPassword };

      await axios.put(`http://localhost:5250/pge/usuarios/atualizar-credenciais`, data, config);

      alert('Credenciais atualizadas com sucesso! Faça login novamente.');
      localStorage.removeItem('token');
      navigate('/');
      
    } catch (error) {
      console.error('Erro ao atualizar credenciais:', error);
      alert('Erro ao atualizar credenciais. Verifique suas informações.');
    }
  };

  const handleNav = () => {
    navigate('/procurador-home')
  }

  return (
    <div className="flex justify-center items-center min-h-screen bg-gray-100 px-4">
      <div className="w-full max-w-md bg-white p-6 rounded-lg shadow-lg">
        <h1 className="text-center text-4xl font-bold text-gray-700 mb-6">Alterar Senha</h1>
        
        <div className="flex flex-col gap-6">
          <input
            type="text"
            placeholder="Usuário"
            value={username}
            onChange={(e) => setUsername(e.target.value)}
            className="w-full px-4 py-3 border rounded-md focus:ring focus:ring-blue-300"
          />
          <input
            type="password"
            placeholder="Senha Atual"
            value={oldPassword}
            onChange={(e) => setOldPassword(e.target.value)}
            className="w-full px-4 py-3 border rounded-md focus:ring focus:ring-blue-300"
          />
          <input
            type="password"
            placeholder="Nova Senha"
            value={newPassword}
            onChange={(e) => setNewPassword(e.target.value)}
            className="w-full px-4 py-3 border rounded-md focus:ring focus:ring-blue-300"
          />
          <button
            onClick={handleUpdate}
            className="w-full px-4 py-3 bg-blue-600 text-white rounded-md  hover:scale-105 transition-all"
          >
            Atualizar Senha
          </button>
          <button
            onClick={handleNav}
            className="w-full px-4 py-3 bg-slate-700 text-white rounded-md hover:scale-105 transition-all"
          >
            Voltar
          </button>
        </div>
      </div>
    </div>
  );
};

export default EditarCredenciais;
