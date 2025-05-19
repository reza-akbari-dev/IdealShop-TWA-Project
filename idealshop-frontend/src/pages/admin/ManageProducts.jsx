import { useEffect, useState } from "react";
import axios from "axios";
import { Link } from "react-router-dom";

function ManageProducts() {
  const [products, setProducts] = useState([]);

  useEffect(() => {
    axios
      .get("https://localhost:7138/api/products")
      .then((res) => setProducts(res.data))
      .catch((err) => console.error(err));
  }, []);

  const handleDelete = (id) => {
    axios
      .delete(`https://localhost:7138/api/products/${id}`)
      .then(() => {
        setProducts(products.filter((p) => p.id !== id));
      })
      .catch((err) => console.error(err));
  };

  return (
    <div className="container mt-4">
      <h3 className="text-primary">Manage Products</h3>
      <Link to="/admin-panel" className="btn btn-outline-secondary mb-3">
        ← Back to Admin Panel
      </Link>

      <div className="text-center mb-4">
        <Link
          to="/admin/products/create"
          className="btn btn-success btn-lg w-50"
        >
          + Add Product
        </Link>
      </div>

      <table className="table table-bordered">
        <thead className="table-light">
          <tr>
            <th>Name</th>
            <th>Category</th>
            <th>Price</th>
            <th>Stock</th>
            <th>Image</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          {products.map((p) => (
            <tr key={p.id}>
              <td>{p.name}</td>
              <td>{p.productCategory?.name}</td>
              <td>${p.price}</td>
              <td>{p.stock}</td>
              <td>
                {p.imageUrl && (
                  <img
                    src={`https://localhost:7138${p.imageUrl}`}
                    alt={p.name}
                    style={{
                      width: "80px",
                      height: "80px",
                      objectFit: "cover",
                    }}
                  />
                )}
              </td>
              <td className="text-center">
                <div className="d-flex justify-content-center gap-2">
                  <Link
                    to={`/admin/products/edit/${p.id}`}
                    className="btn btn-warning w-50"
                  >
                    Edit
                  </Link>
                  <button
                    onClick={() => handleDelete(p.id)}
                    className="btn btn-danger w-50"
                  >
                    Delete
                  </button>
                </div>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}

export default ManageProducts;
