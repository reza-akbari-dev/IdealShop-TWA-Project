import { useEffect, useState } from "react";
import axios from "axios";
import { Link } from "react-router-dom";

function ManageCategories() {
  const [categories, setCategories] = useState([]);
  const [newName, setNewName] = useState("");
  const [editId, setEditId] = useState(null);
  const [editName, setEditName] = useState("");

  useEffect(() => {
    loadCategories();
  }, []);

  const loadCategories = () => {
    axios
      .get("https://localhost:7138/api/categories")
      .then((res) => setCategories(res.data))
      .catch((err) => console.error(err));
  };

  const handleCreate = () => {
    if (!newName.trim()) return;

    axios
      .post("https://localhost:7138/api/categories", { name: newName })
      .then(() => {
        setNewName("");
        loadCategories();
      })
      .catch((err) => console.error(err));
  };

  const handleDelete = (id) => {
    axios
      .delete(`https://localhost:7138/api/categories/${id}`)
      .then(loadCategories)
      .catch((err) => console.error(err));
  };

  const handleEdit = (cat) => {
    setEditId(cat.id);
    setEditName(cat.name);
  };

  const handleUpdate = () => {
    axios
      .put(`https://localhost:7138/api/categories/${editId}`, {
        id: editId,
        name: editName,
      })
      .then(() => {
        setEditId(null);
        setEditName("");
        loadCategories();
      })
      .catch((err) => console.error(err));
  };

  return (
    <div className="container mt-4">
      <h3 className="text-primary">Manage Categories</h3>
      <Link
        to="/admin-panel"
        className="btn btn-outline-secondary mb-3"
        style={{ backgroundColor: "#E8F5E9" }}
      >
        ← Back to Admin Panel
      </Link>

      {/* Create */}
      <div className="input-group mb-3">
        <input
          type="text"
          className="form-control"
          placeholder="New Category Name"
          value={newName}
          onChange={(e) => setNewName(e.target.value)}
        />
        <button className="btn btn-success" onClick={handleCreate}>
          Add
        </button>
      </div>

      {/* List */}
      <ul className="list-group">
        {categories.map((cat) => (
          <li
            key={cat.id}
            className="list-group-item d-flex justify-content-between align-items-center"
          >
            {editId === cat.id ? (
              <div className="d-flex w-100">
                <input
                  className="form-control me-2"
                  value={editName}
                  onChange={(e) => setEditName(e.target.value)}
                />
                <button
                  className="btn btn-sm btn-primary me-2"
                  onClick={handleUpdate}
                >
                  Save
                </button>
                <button
                  className="btn btn-sm btn-secondary"
                  onClick={() => setEditId(null)}
                >
                  Cancel
                </button>
              </div>
            ) : (
              <>
                <span>{cat.name}</span>
                <div>
                  <button
                    className="btn btn-sm btn-warning me-2"
                    onClick={() => handleEdit(cat)}
                  >
                    Edit
                  </button>
                  <button
                    className="btn btn-sm btn-danger"
                    onClick={() => handleDelete(cat.id)}
                  >
                    Delete
                  </button>
                </div>
              </>
            )}
          </li>
        ))}
      </ul>
    </div>
  );
}

export default ManageCategories;
