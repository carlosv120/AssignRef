import React, { useState, useEffect } from "react";
import OfficialTableRow from "./OfficialsItem";
import officialsService from "services/officialsService";
import TitleHeader from "components/general/TitleHeader";
import { Formik, Form, Field } from "formik";
import { Table } from "react-bootstrap";
import { ToastContainer, toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import "rc-pagination/assets/index.css";
import Pagination from "rc-pagination";
import Swal from "sweetalert2";
import "sweetalert2/src/sweetalert2.scss";

import debug from "sabio-debug";
const _logger = debug.extend("Officials");

function Officials() {
  const [officialsData, setOfficialsData] = useState({
    data: [],
    components: [],
    current: 1,
    pageSize: 5,
    total: 0,
  });

  const [isSearching, setIsSearching] = useState(false);

  const [officialsSearch, setOfficialsSearch] = useState({
    searchValue: "",
  });

  useEffect(() => {
    if (isSearching) {
      officialsService
        .getSearch(
          officialsData.current - 1,
          officialsData.pageSize,
          officialsSearch.searchValue
        )
        .then(onGetOfficialSearchSuccess)
        .catch(onGetOfficialSearchError);
    } else {
      getOfficialsData();
    }
  }, [officialsData.current]);

  const getOfficialsData = () => {
    officialsService
      .getAll(officialsData.current - 1, officialsData.pageSize)
      .then(onOfficialsGetAllSuccess)
      .catch(onOfficialsGetAllError);
  };

  const onPageChange = (page) => {
    setOfficialsData((prevState) => {
      const newState = { ...prevState };
      newState.current = page;
      return newState;
    });

    _logger("changing page", officialsData);
  };

  const onOfficialsGetAllSuccess = (response) => {
    setOfficialsData((prevState) => {
      const newState = { ...prevState };
      newState.data = response.item.pagedItems;
      newState.components = response.item.pagedItems.map(mapOfficialTableRow);
      newState.total = response.item.totalCount;
      return newState;
    });
  };

  const onOfficialsGetAllError = (error) => {
    _logger("We could not get the Officials Data! error", error);

    toast.error("We could not find any Official");
  };

  const onGetOfficialSearchSuccess = (response) => {
    setIsSearching(true);
    _logger("my success", response);

    setOfficialsData((prevState) => {
      const newState = { ...prevState };
      newState.data = response.item.pagedItems;
      newState.components = response.item.pagedItems.map(mapOfficialTableRow);
      newState.current = response.item.pageIndex + 1;
      newState.total = response.item.totalCount;
      return newState;
    });
  };

  const onGetOfficialSearchError = (error) => {
    _logger("Error search", error);

    toast.error("We could not find any Official");
  };

  const onSearchBttClicked = (values) => {
    _logger("value to Search btn", values);

    setOfficialsSearch({ searchValue: values.searchValue });

    if (values.searchValue) {
      officialsService
        .getSearch(0, officialsData.pageSize, values.searchValue)
        .then(onGetOfficialSearchSuccess)
        .catch(onGetOfficialSearchError);
    }
  };

  const onClearBttClicked = () => {
    setIsSearching(false);

    setOfficialsSearch({ searchValue: "" });

    getOfficialsData();
  };

  const onDeleteOfficialSuccess = (response) => {
    _logger("Delete success", response);
    Swal.fire("Official status was successfully updated.");

    setOfficialsData((prevState) => {
      const newState = { ...prevState };

      const officialIndex = prevState.data.findIndex(
        (official) => official.id === response
      );

      if (officialIndex >= 0) {
        newState.data[officialIndex] = {
          ...prevState.data[officialIndex],
          statusType: { id: 2, name: "Inactive" },
        };
      }

      newState.components = newState.data.map(mapOfficialTableRow);

      return newState;
    });
  };

  const onDeleteOfficialError = (error) => {
    _logger("Delete error", error);
  };

  const onDeleteClicked = (officialId) => {
    Swal.fire({
      title: "Do you want to delete this Official?",
      showDenyButton: true,
      confirmButtonText: "Yes",
      denyButtonText: `No`,
    }).then((result) => {
      if (result.isConfirmed) {
        const successHandler = onDeleteOfficialSuccess(officialId);
        officialsService
          .delete(officialId)
          .then(successHandler)
          .catch(onDeleteOfficialError);
      } else {
        Swal.fire("Changes are not save");
      }
    });
  };

  const mapOfficialTableRow = (anItem) => {
    return (
      <OfficialTableRow
        key={anItem.id}
        anOfficial={anItem}
        onDelete={onDeleteClicked}
      />
    );
  };

  return (
    <>
      <div>
        <TitleHeader
          title="Officials"
          buttonText="New Official"
          buttonLink="/officials/new"
        />
        <div className="card">
          <Formik
            initialValues={officialsSearch}
            onSubmit={onSearchBttClicked}
            onReset={onClearBttClicked}
          >
            <Form>
              <div className="p-4 d-flex">
                <Field
                  className="form-control form-control-sm"
                  type="text"
                  placeholder="Search Official"
                  name="searchValue"
                />
                <button
                  type="submit"
                  className="btn btn-sm btn-outline-secondary ms-1"
                >
                  Search
                </button>
                {isSearching && (
                  <button type="reset" className="btn btn-outline-warning ms-1">
                    Clear
                  </button>
                )}
              </div>
            </Form>
          </Formik>
          <ToastContainer
            position="top-right"
            autoClose={5000}
            hideProgressBar={false}
            newestOnTop={false}
            closeOnClick
            rtl={false}
            pauseOnFocusLoss
            draggable
            pauseOnHover
            theme="light"
          />
          <Table responsive hover>
            <thead className="table-light">
              <tr>
                <th>User</th>
                <th>Email</th>
                <th>Location</th>
                <th>Phone</th>
                <th>Position</th>
                <th>Conference</th>
                <th className="px-2">Status</th>
                <th className="text-center px-2" colSpan={3}>
                  Options
                </th>
              </tr>
            </thead>
            <tbody>{officialsData.components}</tbody>
          </Table>
          <div className="text-center my-3">
            <Pagination
              onChange={onPageChange}
              current={officialsData.current}
              total={officialsData.total}
              pageSize={officialsData.pageSize}
            />
          </div>
        </div>
      </div>
    </>
  );
}

export default Officials;
