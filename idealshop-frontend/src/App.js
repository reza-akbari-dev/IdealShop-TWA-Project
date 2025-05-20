import React from "react";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import Navbar from "./components/Navbar";
import Home from "./pages/Home";
import Footer from "./components/Footer";
import ProductList from "./pages/ProductList";
import ProductsByCategory from "./pages/ProductsByCategory";
import ProductDetails from "./pages/ProductDetails";
import About from "./pages/About";
import AdminLogin from "./pages/AdminLogin";
import AdminPanel from "./pages/AdminPanel";
import ManageCategories from "./pages/admin/ManageCategories";
import ManageProducts from "./pages/admin/ManageProducts";
import ManageAdmins from "./pages/admin/ManageAdmins";
import ManageCustomers from "./pages/admin/ManageCustomers";
import ProductForm from "./pages/admin/ProductForm";
import CustomerRegister from "./pages/customers/CustomerRegister";
import CustomerLogin from "./pages/customers/CustomerLogin";
import Cart from "./pages/customers/Cart";
import Checkout from "./pages/customers/Checkout";
import OrderPlaced from "./pages/customers/OrderPlaced";

const App = () => {
  return (
    <Router>
      <Navbar />
      <Routes>
        <Route path="/" element={<Home />} />
        <Route path="/products" element={<ProductList />} />
        <Route path="/category/:categoryId" element={<ProductsByCategory />} />
        <Route path="/products/:id" element={<ProductDetails />} />
        <Route path="/about" element={<About />} />
        <Route path="/admin-login" element={<AdminLogin />} />
        <Route path="/admin-panel" element={<AdminPanel />} />
        <Route path="/admin/categories" element={<ManageCategories />} />
        <Route path="/admin/products" element={<ManageProducts />} />
        <Route path="/admin/admins" element={<ManageAdmins />} />
        <Route path="/admin/products/create" element={<ProductForm />} />
        <Route path="/admin/products/edit/:id" element={<ProductForm />} />
        <Route path="/register" element={<CustomerRegister />} />
        <Route path="/login" element={<CustomerLogin />} />
        <Route path="/cart" element={<Cart />} />
        <Route path="/checkout" element={<Checkout />} />
        <Route path="/order-placed" element={<OrderPlaced />} />
        <Route path="/admin/customers" element={<ManageCustomers />} />
      </Routes>
      <Footer />
    </Router>
  );
};

export default App;
