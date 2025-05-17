import { useEffect, useState } from "react";
import { useParams, Link } from "react-router-dom";
import axios from "axios";

function ProductDetails() {
  const { id } = useParams();
  const [product, setProduct] = useState(null);

  useEffect(() => {
    axios
      .get(`https://localhost:7138/api/products/${id}`)
      .then((res) => setProduct(res.data))
      .catch((err) => console.error(err));
  }, [id]);

  if (!product) return <p className="text-center mt-4">Loading...</p>;

  return (
    <div className="container mt-5">
      <h2 className="mb-4">{product.name}</h2>
      <hr />
      <div className="row">
        <div className="col-md-6 text-center">
          <img
            src={`https://localhost:7138${product.imageUrl}`}
            alt={product.name}
            className="card-img-top"
            style={{
              height: "400px",
              width: "100%", // make it fill the column width
              objectFit: "contain", // show full image without cropping or distortion
              borderRadius: "8px", // optional: rounded edges
              backgroundColor: "#f8f9fa", // optional: gray/white background for non-square images
            }}
          />
        </div>
        <div className="col-md-6">
          <p>
            <strong>Category:</strong> {product.productCategory?.name}
          </p>
          <p>
            <strong>Price:</strong> ${product.price.toFixed(2)}
          </p>
          <p>
            <strong>Stock:</strong> {product.stock}
          </p>

          <button className="btn btn-success me-2">Add to Cart</button>
          <Link to="/" className="btn btn-secondary">
            Back to List
          </Link>
        </div>
      </div>
    </div>
  );
}

export default ProductDetails;
