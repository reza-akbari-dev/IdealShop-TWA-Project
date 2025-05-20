// src/pages/customers/Checkout.jsx
import { useNavigate } from "react-router-dom";

function Checkout() {
  const navigate = useNavigate();

  const handleSubmit = (e) => {
    e.preventDefault();

    // You can send payment and shipping data here if needed

    // ✅ Simulate placing order
    navigate("/order-placed");
  };

  return (
    <div className="container mt-4">
      <p className="text-success fw-bold">
        Your order will be delivered to the address associated with your
        account.
      </p>
      <div className="card p-4">
        <h5>💳 Payment Details</h5>
        <form onSubmit={handleSubmit}>
          <input
            className="form-control mb-3"
            placeholder="Name on card"
            required
          />
          <input
            className="form-control mb-3"
            placeholder="Card number"
            required
          />
          <div className="row">
            <div className="col">
              <input
                className="form-control mb-3"
                placeholder="MM/YY"
                required
              />
            </div>
            <div className="col">
              <input className="form-control mb-3" placeholder="CVV" required />
            </div>
          </div>
          <button type="submit" className="btn btn-success">
            Place Your Order
          </button>
        </form>
      </div>
    </div>
  );
}

export default Checkout;
