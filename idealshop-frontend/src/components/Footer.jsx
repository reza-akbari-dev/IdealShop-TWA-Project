import React from "react";

const Footer = () => {
  return (
    <footer
      className="text-center py-3 mt-5"
      style={{
        backgroundColor: "#F48FB1",
        color: "#D50000",
        borderRadius: "20px",
      }}
    >
      <div className="container">
        &copy; 2025 - IdealShop |{" "}
        <a
          href="/about"
          style={{ color: "#D50000", textDecoration: "underline" }}
        >
          About Us
        </a>{" "}
        |{" "}
        <a
          href="/contact"
          style={{ color: "#D50000", textDecoration: "underline" }}
        >
          Contact Us
        </a>
      </div>
    </footer>
  );
};

export default Footer;
