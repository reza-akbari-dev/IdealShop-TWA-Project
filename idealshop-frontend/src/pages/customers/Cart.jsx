import React, { useEffect, useState } from "react";
import axios from "axios";
import { useNavigate } from "react-router-dom";

function Cart() {
  const [cartItems, setCartItems] = useState([]);
  const navigate = useNavigate();

  useEffect(() => {
    axios
      .get("https://localhost:7138/api/cartitems", { withCredentials: true })
      .then((res) => setCartItems(res.data))
      .catch((err) => console.error("Error loading cart items:", err));
  }, []);

  const handleRemove = (id) => {
    axios
      .delete(`https://localhost:7138/api/cartitems/${id}`)
      .then(() => setCartItems(cartItems.filter((item) => item.id !== id)))
      .catch((err) => console.error("Error removing item:", err));
  };

  const total = cartItems.reduce(
    (sum, item) => sum + item.product.price * item.quantity,
    0
  );

  return (
    <div className="container mt-4">
      <h2>🛒 Cart</h2>

      {cartItems.length === 0 ? (
        <p>Your cart is empty.</p>
      ) : (
        <>
          <table className="table table-bordered mt-3">
            <thead className="table-dark">
              <tr>
                <th>Product</th>
                <th>Price</th>
                <th>Quantity</th>
                <th>Total</th>
                <th></th>
              </tr>
            </thead>
            <tbody>
              {cartItems.map((item) => (
                <tr key={item.id}>
                  <td>{item.product.name}</td>
                  <td>${item.product.price}</td>
                  <td>{item.quantity}</td>
                  <td>${(item.product.price * item.quantity).toFixed(2)}</td>
                  <td>
                    <button
                      className="btn btn-danger btn-sm"
                      onClick={() => handleRemove(item.id)}
                    >
                      Remove
                    </button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>

          <h4 className="mt-3">
            Total: <span className="text-success">${total.toFixed(2)}</span>
          </h4>

          <button
            className="btn btn-primary mt-3"
            onClick={() => navigate("/checkout")}
          >
            Proceed to Checkout
          </button>
        </>
      )}
    </div>
  );
}

export default Cart;
