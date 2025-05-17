import React from "react";
import { Link } from "react-router-dom";

function ProductCard({ product }) {
  return (
    <div className="card h-100 shadow">
      <img
        src={product.imageUrl}
        alt={product.name}
        className="card-img-top"
        style={{ height: "200px", objectFit: "cover" }}
      />
      <div className="card-body d-flex flex-column justify-content-between">
        <h5>{product.name}</h5>
        <p className="text-muted">{product.productCategory?.name}</p>
        <p className="fw-bold text-success">${product.price.toFixed(2)}</p>
        <Link
          to={`/products/${product.id}`}
          className="btn btn-primary mt-auto"
        >
          View Details
        </Link>
      </div>
    </div>
  );
}

export default ProductCard;
