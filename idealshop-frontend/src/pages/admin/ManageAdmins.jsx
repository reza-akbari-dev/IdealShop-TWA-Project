import { useEffect, useState } from "react";
import axios from "axios";
import { Link } from "react-router-dom";

function ManageAdmins() {
  const [admins, setAdmins] = useState([]);
  const [form, setForm] = useState({
    firstName: "",
    lastName: "",
    email: "",
    password: "",
    phoneNumber: "",
    address: "",
  });
  const [editId, setEditId] = useState(null);

  useEffect(() => {
    loadAdmins();
  }, []);

  const loadAdmins = () => {
    axios
      .get("https://localhost:7138/api/admin")
      .then((res) => setAdmins(res.data))
      .catch((err) => console.error(err));
  };

  const handleChange = (e) => {
    setForm({ ...form, [e.target.name]: e.target.value });
  };

  const handleSubmit = () => {
    const method = editId ? "put" : "post";
    const url = editId
      ? `https://localhost:7138/api/admin/${editId}`
      : "https://localhost:7138/api/admin";

    axios[method](url, form)
      .then(() => {
        loadAdmins();
        setForm({
          firstName: "",
          lastName: "",
          email: "",
          password: "",
          phoneNumber: "",
          address: "",
        });
        setEditId(null);
      })
      .catch((err) => console.error(err));
  };

  const handleEdit = (admin) => {
    setEditId(admin.id);
    setForm({ ...admin, password: "" }); // Don't load hashed password
  };

  const handleDelete = (id) => {
    axios
      .delete(`https://localhost:7138/api/admin/${id}`)
      .then(() => loadAdmins())
      .catch((err) => console.error(err));
  };

  return (
    <div className="container mt-4">
      <h3 className="text-primary">Manage Admins</h3>
      <Link to="/admin-panel" className="btn btn-outline-secondary mb-3">
        ← Back to Admin Panel
      </Link>

      <div className="row g-2 mb-4">
        {[
          "firstName",
          "lastName",
          "email",
          "password",
          "phoneNumber",
          "address",
        ].map((field) => (
          <div className="col-md-2" key={field}>
            <input
              className="form-control"
              name={field}
              value={form[field]}
              onChange={handleChange}
              placeholder={field.charAt(0).toUpperCase() + field.slice(1)}
              type={field === "password" ? "password" : "text"}
            />
          </div>
        ))}
        <div className="col-md-2">
          <button onClick={handleSubmit} className="btn btn-success w-100">
            {editId ? "Update" : "Add Admin"}
          </button>
        </div>
      </div>

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
          {admins.map((a) => (
            <tr key={a.id}>
              <td>
                {a.firstName} {a.lastName}
              </td>
              <td>{a.email}</td>
              <td>{a.phoneNumber}</td>
              <td>{a.address}</td>
              <td>
                <button
                  className="btn btn-sm btn-warning me-2"
                  onClick={() => handleEdit(a)}
                >
                  Edit
                </button>
                <button
                  className="btn btn-sm btn-danger"
                  onClick={() => handleDelete(a.id)}
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

export default ManageAdmins;
