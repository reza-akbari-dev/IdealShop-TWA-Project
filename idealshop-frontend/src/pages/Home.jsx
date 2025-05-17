import axios from "axios";
import { useEffect, useState } from "react";

function Home() {
  const [products, setProducts] = useState([]);

  useEffect(() => {
    axios
      .get("https://localhost:7138/api/products")
      .then((res) => setProducts(res.data))
      .catch((err) => console.error(err));
  }, []);

  return (
    <div className="container mt-4">
      <h2 className="text-start mb-4 text-primary">🌟 Featured Products</h2>
      <div className="row row-cols-1 row-cols-md-3 g-4">
        {products.map((p) => (
          <div className="col" key={p.id}>
            <div className="card h-100 shadow">
              <img
                src={`https://localhost:7138${p.imageUrl}`}
                className="card-img-top"
                alt={p.name}
                style={{ height: "200px", objectFit: "cover" }}
              />

              <div className="card-body d-flex flex-column">
                <h5 className="card-title">{p.name}</h5>
                <p className="card-text text-muted">
                  {p.productCategory?.name}
                </p>
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

export default Home;
