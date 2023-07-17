import React, { useEffect, useRef } from "react";
import { Chart, registerables } from "chart.js"
import { Card } from "react-bootstrap";;

Chart.register(...registerables);

function LineChart() {
  const chartRef = useRef(null);
  let myChart = null;

  useEffect(() => {
    const myChartRef = chartRef.current.getContext("2d");

    if (myChart) {
      myChart.destroy();
    }

    myChart = new Chart(myChartRef, {
      type: "line",
      data: {
        labels: ["Jan", "Feb", "Mar", "Apr", "May", "Jun"],
        datasets: [
          {
            label: "Earnings",
            data: [20, 35, 40, 25, 45, 60],
            fill: false,
            borderColor: "rgb(75, 192, 192)",
            tension: 0.1,
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
    <Card id="large-card-graph" >
      <Card.Header className="graph-center">Earnings</Card.Header>
      <Card.Body className="graph-center">
      <canvas ref={chartRef} style={{ width: '100%' }}  />
      </Card.Body>
    </Card>
  </React.Fragment>
  );
}

export default LineChart;
