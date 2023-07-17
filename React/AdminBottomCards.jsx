import React from "react";
import { Button, Card, Container } from "react-bootstrap";

function BottomCards() {
  const users = [
    { username: "Alex", gamesRef: "244", fouls: "175 ", conference: "West " },
    { username: "Juan", gamesRef: "4", fouls: "163", conference: "West" },
    { username: "Charles", gamesRef: "23", fouls: "180 ", conference: "East " },
    { username: "John", gamesRef: "2", fouls: "168 ", conference: "West " },
    { username: "Luis", gamesRef: "6", fouls: "190 ", conference: "East " },
  ];

  const usersTwo = [
    { username: "Leslie", gamesRef: "43", fouls: "175 ", conference: "West " },
    { username: "Caleb", gamesRef: "86", fouls: "163 ", conference: "East " },
    { username: "Hector", gamesRef: "32", fouls: "180 ", conference: "East " },
    { username: "Alice", gamesRef: "32", fouls: "168 ", conference: "East " },
    { username: "Mason", gamesRef: "54", fouls: "190 ", conference: "West " },
  ];

  const usersThree = [
    { username: "Ashley", gamesRef: "23", fouls: "175 ", conference: "West " },
    { username: "Harry", gamesRef: "34", fouls: "163 ", conference: "East " },
    { username: "Rodney", gamesRef: "65", fouls: "180 ", conference: "East " },
    { username: "Roddy", gamesRef: "23", fouls: "168 ", conference: "West " },
    { username: "Kim", gamesRef: "13", fouls: "190 ", conference: "West " },
  ];

  function mapUsers(users) {
    return users.map((user) => (
      <div key={user.username} className="card">
        <div className="card-header p-bold-size">{user.username}</div>
        <div className="card-body">
          <p>Games Reffed: {user.gamesRef}</p>
          <p>Fouls: {user.fouls}</p>
          <p>Conference: {user.conference}</p>
        </div>
      </div>
    ));
  }

  function mapUsersTwo(usersTwo) {
    return usersTwo.map((user) => (
      <div key={user.username} className="card">
        <div className="card-header p-bold-size">{user.username}</div>
        <div className="card-body">
          <p>Games Reffed: {user.gamesRef}</p>
          <p>Fouls: {user.fouls}</p>
          <p>Conference: {user.conference}</p>
        </div>
      </div>
    ));
  }

  function mapUsersThree(usersThree) {
    return usersThree.map((user) => (
      <div key={user.username} className="card">
        <div className="card-header p-bold-size">{user.username}</div>
        <div className="card-body">
          <p>Games Reffed: {user.gamesRef}</p>
          <p>Fouls: {user.fouls}</p>
          <p>Conference: {user.conference}</p>
        </div>
      </div>
    ));
  }

  return (
    <React.Fragment>
      <Container style={{ display: "flex", flexDirection: "row" }}>
        <div className="row instructor-bottom-cards">
          <div className="col">
            <Card className="card-margintop">
              <Card.Header
                className="bottom-cards-align"
                style={{ justifyContent: "space-between" }}
              >
                Popular Referees
                <Button variant="success">view all</Button>
              </Card.Header>
              <div className="main-card">
                <div className="card-stack">{mapUsers(users)}</div>
              </div>
            </Card>
          </div>

          <div className="col">
            <Card className="card-margintop">
              <Card.Header className="bottom-cards-align">
                Popular Referees
                <Button variant="success">view all</Button>
              </Card.Header>
              <div className="main-card">
                <div className="card-stack">{mapUsersTwo(usersTwo)}</div>
              </div>
            </Card>
          </div>

          <div className="col">
            <Card className="card-margintop">
              <Card.Header className="bottom-cards-align">
                Popular Referees
                <Button variant="success">view all</Button>
              </Card.Header>
              <div className="main-card">
                <div className="card-stack">{mapUsersThree(usersThree)}</div>
              </div>
            </Card>
          </div>
        </div>
      </Container>
    </React.Fragment>
  );
}

export default BottomCards;