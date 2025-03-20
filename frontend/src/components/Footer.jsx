import { useState, useEffect } from "react";

const Footer = () => {
  const [currentDateTime, setCurrentDateTime] = useState("");

  useEffect(() => {
    const updateDateTime = () => {
      const now = new Date();
      setCurrentDateTime(now.toLocaleString()); 
    };

    updateDateTime(); 

    const intervalId = setInterval(updateDateTime, 1000); 

 
    return () => clearInterval(intervalId);
  }, []);

  return (
    <footer className="bg-black text-white py-4  bottom-0 w-full">
      <div className="flex justify-between">
        <p className="ml-4">{currentDateTime}</p> 
      </div>
    </footer>
  );
};

export default Footer;
