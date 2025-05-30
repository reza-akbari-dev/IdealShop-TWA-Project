import React, { useEffect, useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import axios from "axios";

function Navbar() {
  const [categories, setCategories] = useState([]);
  const [isAdminLoggedIn, setIsAdminLoggedIn] = useState(false);
  const [isCustomerLoggedIn, setIsCustomerLoggedIn] = useState(false);
  const navigate = useNavigate();

  useEffect(() => {
    axios
      .get("https://localhost:7138/api/categories")
      .then((res) => setCategories(res.data))
      .catch((err) => console.error("Failed to load categories:", err));

    const isAdmin = localStorage.getItem("isAdmin") === "true";
    const isCustomer = localStorage.getItem("isCustomer") === "true";

    setIsAdminLoggedIn(isAdmin);
    setIsCustomerLoggedIn(isCustomer);
  }, []);

  const handleCategoryClick = (categoryId) => {
    navigate(`/category/${categoryId}`);
  };

  const handleLogout = () => {
    localStorage.removeItem("isAdmin");
    localStorage.removeItem("isCustomer");
    setIsAdminLoggedIn(false);
    setIsCustomerLoggedIn(false);
    navigate("/");
    window.location.reload(); // refresh to update nav state
  };

  return (
    <nav className="navbar navbar-expand-lg navbar-dark bg-primary px-4 rounded-bottom">
      <Link className="navbar-brand fw-bold text-warning me-4" to="/">
        <span
          style={{
            fontFamily: "cursive",
            fontSize: "1.5rem",
            color: "#FF5722",
          }}
        >
          IdealShop
        </span>
      </Link>

      <div className="collapse navbar-collapse">
        <ul className="navbar-nav me-auto">
          <li className="nav-item">
            <Link className="nav-link" to="/">
              Home
            </Link>
          </li>

          <li className="nav-item">
            <Link className="nav-link" to="/products">
              Products
            </Link>
          </li>

          <li className="nav-item dropdown">
            <button
              className="nav-link dropdown-toggle btn btn-link text-white"
              type="button"
              data-bs-toggle="dropdown"
            >
              Categories
            </button>
            <ul className="dropdown-menu">
              {categories.length > 0 ? (
                categories.map((cat) => (
                  <li key={cat.id}>
                    <button
                      className="dropdown-item"
                      onClick={() => handleCategoryClick(cat.id)}
                    >
                      {cat.name}
                    </button>
                  </li>
                ))
              ) : (
                <li>
                  <span className="dropdown-item text-muted">
                    No categories
                  </span>
                </li>
              )}
            </ul>
          </li>

          <li className="nav-item">
            <Link className="nav-link" to="/about">
              About Us
            </Link>
          </li>
        </ul>

        <div className="d-flex">
          {isAdminLoggedIn || isCustomerLoggedIn ? (
            <>
              {isCustomerLoggedIn && (
                <Link className="btn btn-success me-2" to="/cart">
                  Cart
                </Link>
              )}
              <button className="btn btn-danger" onClick={handleLogout}>
                Sign Out
              </button>
            </>
          ) : (
            <>
              <Link className="btn btn-primary me-2" to="/admin-login">
                Admin Login
              </Link>
              <Link className="btn btn-primary me-2" to="/login">
                Login
              </Link>
              <Link className="btn btn-primary" to="/register">
                Register
              </Link>
            </>
          )}
        </div>
      </div>
    </nav>
  );
}

export default Navbar;
