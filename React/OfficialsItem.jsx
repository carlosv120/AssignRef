import React from "react";
import { FaUserEdit } from "react-icons/fa";
import { BsTrash3 } from "react-icons/bs";
import { SlOptionsVertical } from "react-icons/sl";
import {
  Dropdown,
  DropdownToggle,
  DropdownMenu,
  DropdownItem,
} from "reactstrap";
import Badge from "react-bootstrap/Badge";
import PropTypes from "prop-types";

const OfficialTableRow = ({ anOfficial, onDelete }) => {
  const [showDropdown, setShowDropdown] = React.useState(false);

  const isActive = anOfficial.statusType.id === 1;

  const conferencesArray = anOfficial.conferences?.map((item) => {
    return (
      <Badge bg="primary" key={item.id} className="me-1 fw-bold">
        {item.code}
      </Badge>
    );
  });

  const onDeleteIconClicked = (evt) => {
    evt.preventDefault();
    onDelete(anOfficial.id);
  };

  const toggleModal = () => {
    setShowDropdown(!showDropdown);
  };

  return (
    <tr>
      <td>
        <img
          src={anOfficial.user.avatarUrl}
          alt="pic"
          className="rounded-circle border border-white me-3"
          style={{ height: "2.2rem", width: "2.2rem" }}
        />
        <span className="fw-bold text-secondary">
          {anOfficial.user.lastName}, {anOfficial.user.firstName}
        </span>
      </td>
      <td>{anOfficial.email}</td>
      <td>{`${anOfficial.location?.lineOne}, ${anOfficial.location?.city}, ${anOfficial.location?.state?.code}`}</td>
      <td>{anOfficial.phone}</td>
      <td>{anOfficial.primaryPosition.name}</td>
      <td>{conferencesArray}</td>
      <td className="text-center">
        <span className="me-1">
          <span
            className={`badge-dot badge ${isActive ? "bg-success" : "bg-danger"
              }`}
          ></span>{" "}
        </span>
      </td>
      <td className="text-center">
        <Dropdown isOpen={showDropdown} toggle={toggleModal}>
          <DropdownToggle aria-expanded data-toggle="dropdown" tag="span">
            <SlOptionsVertical title="More Options" />
          </DropdownToggle>
          <DropdownMenu className="opacity-100">
            <DropdownItem>
              <FaUserEdit title="Edit" className="text-warning" /> Edit
            </DropdownItem>
            {isActive && (
              <DropdownItem onClick={onDeleteIconClicked}>
                <BsTrash3 title="Delete" className="text-danger" /> Delete
              </DropdownItem>
            )}
          </DropdownMenu>
        </Dropdown>
      </td>
    </tr>
  );
};

OfficialTableRow.propTypes = {
  anOfficial: PropTypes.shape({
    id: PropTypes.number.isRequired,
    phone: PropTypes.string,
    email: PropTypes.string,
    user: PropTypes.shape({
      avatarUrl: PropTypes.string.isRequired,
      firstName: PropTypes.string.isRequired,
      lastName: PropTypes.string.isRequired,
    }).isRequired,
    location: PropTypes.shape({
      lineOne: PropTypes.string.isRequired,
      city: PropTypes.string.isRequired,
      state: PropTypes.shape({
        code: PropTypes.string,
      }),
    }).isRequired,
    primaryPosition: PropTypes.shape({
      name: PropTypes.string.isRequired,
    }).isRequired,
    statusType: PropTypes.shape({
      id: PropTypes.number.isRequired,
      name: PropTypes.string.isRequired,
    }).isRequired,
    conferences: PropTypes.arrayOf(
      PropTypes.shape({
        id: PropTypes.number.isRequired,
        name: PropTypes.string.isRequired,
        code: PropTypes.string.isRequired,
        logo: PropTypes.string.isRequired,
      }).isRequired
    ),
  }).isRequired,
  onDelete: PropTypes.func.isRequired,
};

export default OfficialTableRow;
