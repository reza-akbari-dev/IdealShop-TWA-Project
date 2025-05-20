import { useEffect, useState } from "react";
import axios from "axios";
import { Link } from "react-router-dom";

function ManageCustomers() {
  const [customers, setCustomers] = useState([]);

  useEffect(() => {
    axios
      .get("https://localhost:7138/api/customers")
      .then((res) => setCustomers(res.data))
      .catch((err) => console.error("Error loading customers:", err));
  }, []);

  const handleDelete = (id) => {
    if (!window.confirm("Are you sure you want to delete this customer?"))
      return;

    axios
      .delete(`https://localhost:7138/api/customers/${id}`)
      .then(() => setCustomers(customers.filter((c) => c.id !== id)))
      .catch((err) => console.error("Delete error:", err));
  };

  return (
    <div className="container mt-4">
      <h3 className="text-primary">Manage Customers</h3>
      <Link to="/admin-panel" className="btn btn-outline-secondary mb-3">
        ← Back to Admin Panel
      </Link>

      <table className="table table-bordered">
        <thead className="table-light">
          <tr>
            <th>Name</th>
            <th>Email</th>
            <th>Phone</th>
            <th>Address</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          {customers.map((c) => (
            <tr key={c.id}>
              <td>
                {c.firstName} {c.lastName}
              </td>
              <td>{c.email}</td>
              <td>{c.phoneNumber}</td>
              <td>{c.address}</td>
              <td>
                <button
                  className="btn btn-sm btn-danger"
                  onClick={() => handleDelete(c.id)}
                >
                  Delete
                </button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}

export default ManageCustomers;
