import React from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import {
  faTv,
  faBookOpen,
  faUsers,
  faUserCheck,
} from "@fortawesome/free-solid-svg-icons";
import PropTypes from "prop-types";

function AdminDbCard({ data }) {
  const topCards = [
    {
      name: "CONFERENCES",
      icon: faBookOpen,
      number: data?.totalConferences,
      text: "Conferences",
      color: "info",
    },
    {
      name: "GAMES",
      icon: faTv,
      number: data?.totalGames,
      text: "Games",
      color: "success",
    },
    {
      name: "OFFICIALS",
      icon: faUsers,
      number: data?.totalOfficials,
      text: "Officials",
      color: "warning",
    },
    {
      name: "USERS",
      icon: faUserCheck,
      number: data?.totalUsers,
      text: "Users",
      color: "danger",
    },
  ];

  function mapTopCards(topCards) {
    return topCards.map((topCard) => (
      <div className="col-xl-3 col-lg-6 col-md-12 col-sm-12" key={topCard.name}>
        <div className="mb-3 card border-light">
          <div className="card-body py-3 px-4">
            {topCard.name === "USERS" ? (
              <a
                href={`/admin/useradminview`}
                className="fs-6 text-uppercase fw-semi-bold"
              >
                {topCard.name}
              </a>
            ) : (
              <a
                href={`/${topCard.name.toLowerCase()}`}
                className="fs-6 text-uppercase fw-semi-bold"
              >
                {topCard.name}
              </a>
            )}
            <div className="mt-2 d-flex justify-content-between align-items-center">
              <div className="lh-1">
                <h2 className="h1 fw-bold mb-1">{topCard.number}</h2>
              </div>
              <div>
                <span
                  className={`bg-light-${topCard.color} icon-shape icon-lg rounded-3 text-dark-${topCard.color}`}
                >
                  <div>
                    <FontAwesomeIcon icon={topCard.icon} size="2xl" />
                  </div>
                </span>
              </div>
            </div>
          </div>
        </div>
      </div>
    ));
  }

  return (
    <React.Fragment>
      <div>
        <div className="row dashboard-botton-cards">
          {mapTopCards(topCards)}
        </div>
      </div>
    </React.Fragment>
  );
}

AdminDbCard.propTypes = {
  data: PropTypes.shape({
    totalConferences: PropTypes.number,
    totalGames: PropTypes.number,
    totalOfficials: PropTypes.number,
    totalUsers: PropTypes.number,
  }),
};

export default AdminDbCard;
