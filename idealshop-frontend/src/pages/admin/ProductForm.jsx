import { useEffect, useState } from "react";
import axios from "axios";
import { useNavigate, useParams } from "react-router-dom";

function ProductForm() {
  const { id } = useParams(); // if editing
  const navigate = useNavigate();
  const [form, setForm] = useState({
    name: "",
    price: "",
    stock: "",
    productCategoryId: "",
    imageFile: null,
  });
  const [categories, setCategories] = useState([]);

  useEffect(() => {
    axios
      .get("https://localhost:7138/api/categories")
      .then((res) => setCategories(res.data));
    if (id) {
      axios.get(`https://localhost:7138/api/products/${id}`).then((res) => {
        const p = res.data;
        setForm({
          name: p.name,
          price: p.price,
          stock: p.stock,
          productCategoryId: p.productCategoryId,
          imageFile: null,
        });
      });
    }
  }, [id]);

  const handleChange = (e) => {
    const { name, value, files } = e.target;
    if (name === "imageFile") {
      setForm({ ...form, imageFile: files[0] });
    } else {
      setForm({ ...form, [name]: value });
    }
  };

  const handleSubmit = () => {
    const formData = new FormData();
    formData.append("name", form.name);
    formData.append("price", form.price);
    formData.append("stock", form.stock);
    formData.append("productCategoryId", form.productCategoryId);
    if (form.imageFile) formData.append("imageFile", form.imageFile);

    const request = id
      ? axios.put(`https://localhost:7138/api/products/${id}`, formData)
      : axios.post("https://localhost:7138/api/products", formData);

    request
      .then(() => navigate("/admin/products"))
      .catch((err) => console.error(err));
  };

  return (
    <div className="container mt-4">
      <h3>{id ? "Edit Product" : "Create Product"}</h3>

      <div className="row g-3">
        <div className="col-md-6">
          <input
            className="form-control"
            name="name"
            placeholder="Name"
            value={form.name}
            onChange={handleChange}
          />
        </div>
        <div className="col-md-6">
          <input
            className="form-control"
            name="price"
            placeholder="Price"
            value={form.price}
            onChange={handleChange}
          />
        </div>
        <div className="col-md-6">
          <input
            className="form-control"
            name="stock"
            placeholder="Stock"
            value={form.stock}
            onChange={handleChange}
          />
        </div>
        <div className="col-md-6">
          <select
            className="form-select"
            name="productCategoryId"
            value={form.productCategoryId}
            onChange={handleChange}
          >
            <option value="">Select Category</option>
            {categories.map((c) => (
              <option key={c.id} value={c.id}>
                {c.name}
              </option>
            ))}
          </select>
        </div>
        <div className="col-md-6">
          <input
            type="file"
            className="form-control"
            name="imageFile"
            onChange={handleChange}
          />
        </div>
        <div className="col-md-6">
          <button className="btn btn-primary w-100" onClick={handleSubmit}>
            {id ? "Update Product" : "Add Product"}
          </button>
        </div>
      </div>
    </div>
  );
}

export default ProductForm;
