import React, { useState, useEffect, useRef } from 'react';
import { MapContainer, TileLayer, Polygon, MapContainerProps } from 'react-leaflet';
import 'leaflet/dist/leaflet.css';
import * as h3 from 'h3-js';
import L from 'leaflet';

interface H3MapProps extends MapContainerProps {
  center: L.LatLngExpression;
  zoom: number;
}

const H3Map: React.FC<H3MapProps> = ({ center, zoom }) => {
  const [visibleHexagons, setVisibleHexagons] = useState<string[]>([]);
  const [extendedBounds, setExtendedBounds] = useState<number[][]>([]);
  const [map, setMap] = useState<L.Map | null>(null);

  const getResolution = (zoom: number) => {
    console.log("zoom: " + zoom);
    const resolutions = [0, 0, 1, 1, 1, 2, 2, 3, 3, 4, 5, 6, 6, 7, 7, 8, 9, 10, 11];
    return resolutions[zoom] ?? 10;
  };

  const updateVisibleHexagons = () => {
    if (!map) return;

    const bounds = map.getBounds();
    const ne = bounds.getNorthEast();
    const sw = bounds.getSouthWest();

    const clampLat = (lat : number) => Math.max(-90, Math.min(90, lat));
    const clampLng = (lng : number) => Math.max(-180, Math.min(180, lng));

    const extendedBounds = [
      [clampLat(sw.lat), clampLng(sw.lng)],
      [clampLat(sw.lat), clampLng(ne.lng)],
      [clampLat(ne.lat), clampLng(ne.lng)],
      [clampLat(ne.lat), clampLng(sw.lng)],
      [clampLat(sw.lat), clampLng(sw.lng)]
    ];

    const resolution = getResolution(map.getZoom());

    setExtendedBounds(extendedBounds);
    console.log(extendedBounds);

    const hexagons = h3.polygonToCells(extendedBounds, resolution);
    console.log(hexagons);
    setVisibleHexagons(hexagons);
  };

  const handleMoveEnd = () => {
    updateVisibleHexagons();
  };

  useEffect(() => {
    if (map) {
      map.on('moveend', handleMoveEnd);
      updateVisibleHexagons(); // Initial update
    }
    return () => {
      if (map) {
        map.off('moveend', handleMoveEnd);
      }
    };
  }, [map]);

  useEffect(() => {
    if (map) {
      updateVisibleHexagons();
    }
  }, [zoom]);

  return (
    <MapContainer
      worldCopyJump={true}
      center={center}
      zoom={zoom}
      style={{ height: '100vh', width: '100%' }}
      ref={setMap}
      maxZoom={18}
      minZoom={4}
    >
      <TileLayer
        // noWrap={true}
        url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
        attribution='&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
      />
      {visibleHexagons.map(hex => {
        const hexBoundary = h3.cellToBoundary(hex, true);
        return (
          <Polygon
            key={hex}
            positions={hexBoundary.map(([lng, lat]: [number, number]) => [lat, lng]) as [number, number][]}
            color="blue"
          />
        );
      })}
      {extendedBounds.length > 0 && (
        <Polygon
          positions={extendedBounds as [number, number][]}
          color="red"
        />
      )}
    </MapContainer>
  );
};

export default H3Map;
