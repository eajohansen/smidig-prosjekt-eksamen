import React from "react";
import { Link } from "react-router-dom";

export const EventItem = (props) => {
  const { title, date, id } = props;
  return (
    <article key={id} className="eventItemContainer">
      <img
        src="https://images.unsplash.com/photo-1717684566059-4d16b456c72a?q=80&w=2938&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D"
        alt=""
      />
      <div className="eventItemText">
        <p>{title}</p>
        <p>{date}</p>
      </div>
      <Link to={`/events/details/${id}`}>
        <button>Se arrangement</button>
      </Link>
    </article>
  );
};
