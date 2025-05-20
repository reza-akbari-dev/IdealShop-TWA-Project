import { useState } from "react";
import axios from "axios";
import { useNavigate } from "react-router-dom";

function CustomerLogin() {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");
  const navigate = useNavigate();

  const handleLogin = async (e) => {
    e.preventDefault();

    try {
      await axios.post(
        "https://localhost:7138/api/customers/login",
        { email, password },
        { withCredentials: true } // ✅ This sends the cookie
      );

      localStorage.setItem("isCustomer", "true");
      localStorage.setItem("customerEmail", email); // ✅ Add this
      navigate("/");
      window.location.reload(); // 🔁 Force navbar to update
    } catch (err) {
      setError("Invalid credentials");
    }
  };

  return (
    <div className="container mt-5" style={{ maxWidth: "500px" }}>
      <h2 className="text-center mb-4 text-primary">Customer Login</h2>
      {error && <div className="alert alert-danger">{error}</div>}
      <form onSubmit={handleLogin}>
        <input
          type="email"
          className="form-control mb-3"
          placeholder="Email"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
          required
        />
        <input
          type="password"
          className="form-control mb-3"
          placeholder="Password"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          required
        />
        <button type="submit" className="btn btn-primary w-100">
          Login
        </button>
      </form>
    </div>
  );
}

export default CustomerLogin;
