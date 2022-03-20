import React from "react";

const UiBotao = (props) => {
  const { temIconeBotao, classeIconeBotao, labelBotao, onClickBotao } = props;

  return <button onClick={onClickBotao}>{labelBotao}</button>;
};

export default UiBotao;
