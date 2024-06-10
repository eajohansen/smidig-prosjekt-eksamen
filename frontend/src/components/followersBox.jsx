import React from "react";
//import { Link } from "react-router-dom";
export const followersBox = (props) => {
    const { followersTotal, followersLastMonth, growth } = props;
    return (
        <article className="followerItemContainer">
            <h1>VEKST</h1>
            <h2>Følgere:</h2>
            <p>{followersTotal}</p>
            <h2>Følgere forrige måned:</h2>
            <p>{followersLastMonth}</p>
            <h2>Vekst:</h2>
            <p>{growth}</p>
        </article>
    );
};
