import axios from "axios";

const API_URL = "http://localhost:5250"; 


export const registerUser = async (userData) => {
  try {
    
    if (!userData.username || !userData.password || !userData.role) {
      throw new Error("Campos obrigatórios ausentes.");
    }

 
    const userPayload = {
      username: userData.username,
      password: userData.password,
      role: userData.role,
      nome: userData.role === "Cliente" ? userData.nome : "", 
      oab: userData.role === "Procurador" ? userData.oab : "", 
    };


    console.log("Payload enviado:", userPayload);


    const response = await axios.post(`${API_URL}/pge/usuarios/register`, userPayload);

  
    return response.data;
  } catch (error) {
    
    console.error("Erro ao registrar usuário:", error);

    
    throw error.response?.data || error.message || "Erro ao registrar usuário.";
  }
};


export const loginUser = async (credentials) => {
  try {
    const response = await axios.post(`${API_URL}/pge/usuarios/login`, credentials);
    return response.data;
  } catch (error) {
    throw error.response?.data || "Erro ao fazer login.";
  }
};


export const updateCredentials = async (updateData, token) => {
  try {
    const response = await axios.put(`${API_URL}/pge/atualizar-credenciais`, updateData, {
      headers: { Authorization: `Bearer ${token}` },
    });
    return response.data;
  } catch (error) {
    throw error.response?.data || "Erro ao atualizar credenciais.";
  }
};
