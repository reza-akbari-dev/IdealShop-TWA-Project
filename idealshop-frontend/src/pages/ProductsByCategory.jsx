import { useParams } from "react-router-dom";
import { useEffect, useState } from "react";
import axios from "axios";

function ProductsByCategory() {
  const { categoryId } = useParams();
  const [products, setProducts] = useState([]);

  useEffect(() => {
    axios
      .get(`https://localhost:7138/api/products/bycategory/${categoryId}`)
      .then((res) => setProducts(res.data))
      .catch((err) => console.error(err));
  }, [categoryId]);

  return (
    <div className="container mt-4">
      <h2 className="text-primary mb-4">Products in Category</h2>
      <div className="row row-cols-1 row-cols-md-3 g-4">
        {products.map((p) => (
          <div className="col" key={p.id}>
            <div className="card h-100 shadow-sm">
              <img
                src={`https://localhost:7138${p.imageUrl}`}
                alt={p.name}
                className="card-img-top"
                style={{ height: "200px", objectFit: "cover" }}
              />
              <div className="card-body d-flex flex-column justify-content-between">
                <h5 className="card-title">{p.name}</h5>
                <p className="text-muted">{p.productCategory?.name}</p>
                <p className="fw-bold text-success">${p.price.toFixed(2)}</p>
                <a
                  href={`/products/${p.id}`}
                  className="btn btn-primary mt-auto"
                >
                  View Details
                </a>
              </div>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
}

export default ProductsByCategory;
