import React, { useEffect, useRef } from "react";
import { Chart, registerables } from "chart.js";
import { Card } from "react-bootstrap";

Chart.register(...registerables);

function PieChart() {
  const chartRef = useRef(null);
  let myChart = null;

  useEffect(() => {
    const myChartRef = chartRef.current.getContext("2d");

    if (myChart) {
      myChart.destroy();
    }

    myChart = new Chart(myChartRef, {
      type: "pie",
      data: {
        labels: ["False Start", "Holding", "Pass Interference"],
        datasets: [
          {
            label: "Fouls",
            data: [300, 50, 100],
            backgroundColor: [
              "rgb(255, 99, 132)",
              "rgb(54, 162, 235)",
              "rgb(255, 205, 86)",
            ],
          },
        ],
      },
      options: {},
    });

    return () => {
      if (myChart) {
        myChart.destroy();
      }
    };
  }, []);

  return (
    <React.Fragment>
      <Card id="small-card-pie">
        <Card.Header>Fouls</Card.Header>
        <Card.Body>
        <canvas ref={chartRef} />
        </Card.Body>
      </Card>
    </React.Fragment>
  );
}

export default PieChart;