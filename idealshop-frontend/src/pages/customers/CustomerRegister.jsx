import { useState } from "react";
import axios from "axios";
import { useNavigate } from "react-router-dom";

function CustomerRegister() {
  const [form, setForm] = useState({
    firstName: "",
    lastName: "",
    email: "",
    password: "",
    phoneNumber: "",
    address: "",
  });

  const navigate = useNavigate();

  const handleChange = (e) => {
    setForm({ ...form, [e.target.name]: e.target.value });
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    axios
      .post("https://localhost:7138/api/customers", form)
      .then(() => navigate("/"))
      .catch((err) => console.error(err));
  };

  return (
    <div className="container mt-5" style={{ maxWidth: "600px" }}>
      <h2 className="bg-success text-white text-center p-2 rounded-top">
        Customer Registration
      </h2>
      <form onSubmit={handleSubmit} className="border p-4">
        {[
          "firstName",
          "lastName",
          "email",
          "password",
          "phoneNumber",
          "address",
        ].map((field) => (
          <div className="mb-3" key={field}>
            <label className="form-label">{field}</label>
            <input
              className="form-control"
              name={field}
              type={field === "password" ? "password" : "text"}
              value={form[field]}
              onChange={handleChange}
              required
            />
          </div>
        ))}
        <button className="btn btn-success w-100">Register</button>
      </form>
    </div>
  );
}

export default CustomerRegister;
