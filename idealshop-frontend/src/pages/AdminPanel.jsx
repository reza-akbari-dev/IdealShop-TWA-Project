import React from "react";

function AdminPanel() {
  return (
    <div className="container mt-5">
      <h2 className="text-primary mb-4">🔒 Admin Dashboard</h2>
      <p>
        Welcome, admin! Use the menu to manage products, categories, customers,
        and other admins.
      </p>

      <ul className="list-group">
        <li className="list-group-item">
          <a href="/admin/categories">Manage Categories</a>
        </li>
        <li className="list-group-item">
          <a href="/admin/products">Manage Products</a>
        </li>
        <li className="list-group-item">
          <a href="/admin/customers">Manage Customers</a>
        </li>
        <li className="list-group-item">
          <a href="/admin/admins">Manage Admins</a>
        </li>
      </ul>
    </div>
  );
}

export default AdminPanel;
