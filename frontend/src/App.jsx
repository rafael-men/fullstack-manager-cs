import React from "react"
import { BrowserRouter as Router, Routes,Route, Navigate} from 'react-router-dom'
import Login from "./pages/Login"
import Register from "./pages/Register"
import Navbar from "./components/Navbar"
import Footer from "./components/Footer"
import HomeProcurador from "./pages/Home_Procurador"
import EditarCredenciais from "./pages/editCredentials"
import Home from "./pages/Home"
import EditarCredenciaisCliente from "./pages/editCredentialsCliente"
import CriarProcesso from "./pages/CriarProcesso"
import CriarDocumento from "./pages/CriarDocumento"
import CriarPrazo from "./pages/CriarPrazo"
import EditarProcesso from "./pages/EditProcesso"


function App() {
 
  return (
    <Router>
      <Navbar/>
      <Routes>
        <Route path="/" element={<Navigate to="/login" />} />
        <Route path="/login" element={<Login />} />
        <Route path="/register" element={<Register />} />
        <Route path="/procurador-home" element={<HomeProcurador/>}/>
        <Route path="/editar-cadastro" element={<EditarCredenciais/>}/>
        <Route path="/home" element={<Home/>}/>
        <Route path="/editar-cadastro-cliente" element={<EditarCredenciaisCliente/>}/>
        <Route path="/processos/criar" element={<CriarProcesso/>}/>
        <Route path="/documentos/criar" element={<CriarDocumento/>}/>
        <Route path="/prazos/criar" element={<CriarPrazo/>}/>
        <Route path='/processos/editar/:numero' element={<EditarProcesso/>}/>
      </Routes>
      <Footer/>
    </Router>
  )
}

export default App
