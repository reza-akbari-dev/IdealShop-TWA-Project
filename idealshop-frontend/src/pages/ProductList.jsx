import axios from "axios";
import { useEffect, useState } from "react";

function ProductList() {
  const [products, setProducts] = useState([]);

  useEffect(() => {
    axios
      .get("https://localhost:7138/api/products")
      .then((res) => setProducts(res.data))
      .catch((err) => console.error(err));
  }, []);

  return (
    <div className="container mt-5">
      <h2 className="mb-4">Product List</h2>
      <table className="table table-bordered table-striped">
        <thead className="table-dark">
          <tr>
            <th>Name</th>
            <th>Category</th>
            <th>Price</th>
            <th>Stock</th>
            <th>Image</th>
            <th>Details</th>
          </tr>
        </thead>
        <tbody>
          {products.map((p) => (
            <tr key={p.id}>
              <td>{p.name}</td>
              <td>{p.productCategory?.name}</td>
              <td>{p.price}</td>
              <td>{p.stock}</td>
              <td>
                <img
                  src={`https://localhost:7138${p.imageUrl}`}
                  alt={p.name}
                  width="60"
                />
              </td>
              <td>
                <a
                  href={`/products/${p.id}`}
                  className="btn btn-outline-info btn-sm"
                >
                  View
                </a>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}

export default ProductList;
