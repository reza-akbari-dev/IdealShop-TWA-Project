// src/pages/customers/OrderPlaced.jsx
import { useNavigate } from "react-router-dom";

function OrderPlaced() {
  const navigate = useNavigate();

  return (
    <div className="container mt-5 text-center">
      <h3 className="text-success">✅ Order Placed Successfully!</h3>
      <p>
        Your order was placed successfully and will be delivered to the address
        associated with your account.
      </p>
      <button className="btn btn-primary" onClick={() => navigate("/")}>
        Continue Shopping
      </button>
    </div>
  );
}

export default OrderPlaced;
