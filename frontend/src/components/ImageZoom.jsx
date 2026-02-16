
// import React, { useState } from "react";
// import PropTypes from "prop-types";

// const ImageZoom = ({ options, compressedImage , originalImage ,dir,zoomFactor }) => {
//   const [isZoomed, setIsZoomed] = useState(false);
//   const [mousePosition, setMousePosition] = useState({ x: 0, y: 0 });

//   const handleMouseEnter = (e) => {
//     setIsZoomed(true);
//     handleMouseMove(e);
//   };

//   const handleMouseMove = (e) => {
//     const { left, top, width, height } = e.target.getBoundingClientRect();
//     const x = (e.clientX - left) / width * 100;
//     const y = (e.clientY - top) / height * 100;
//     setMousePosition({ x, y });
//   };

//   const handleMouseLeave = () => {
//     setIsZoomed(false);
//   };

//   const zoomedImageStyle = {
//     backgroundImage: `url(${originalImage || compressedImage})`,
//     backgroundPosition: `${mousePosition.x}% ${mousePosition.y}%`,
//     display: isZoomed ? "block" : "none",
//   };

//   return (
//     <div className={`image-container ${dir}`}>
//       <div
//         className="image-wrapper"
//         onMouseMove={handleMouseMove}
//         onMouseLeave={handleMouseLeave}
//         onMouseEnter={handleMouseEnter}
//       >
//         <img className="sampleImage" src={compressedImage} alt="sample" />
//         <div id="zoomedImageContainer" className="zoomedImageContainer" style={zoomedImageStyle} />
//       </div>
//     </div>
//   );
// };

// ImageZoom.propTypes = {
//   options: PropTypes.shape({
//     originalImage: PropTypes.string.isRequired,
//     compressedImage: PropTypes.string.isRequired,
//     dir: PropTypes.string.isRequired,
//     zoomFactor: PropTypes.string,
//   }),
// };

// export default ImageZoom;

// const [isZoomed, setIsZoomed] = useState(false);
// const [mousePosition, setMousePosition] = useState({ x: 0, y: 0 });

// const handleMouseEnter = (e) => {
//   setIsZoomed(true);
//   handleMouseMove(e);
// };

// const handleMouseMove = (e) => {
//   const { left, top, width, height } = e.target.getBoundingClientRect();
//   const x = (e.clientX - left) / width * 100;
//   const y = (e.clientY - top) / height * 100;
//   setMousePosition({ x, y });
// };

// const handleMouseLeave = () => {
//   setIsZoomed(false);
// };







