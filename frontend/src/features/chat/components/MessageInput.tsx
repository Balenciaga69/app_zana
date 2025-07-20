// 訊息輸入元件
import { HStack, Input, IconButton, useColorMode } from "@chakra-ui/react";
import { useState } from "react";
import { useChatStore } from "../store/chatStore";
import { FiSend, FiCamera } from "react-icons/fi";
import { messageInputContainerStyles, messageInputStyles } from "../styles/componentStyles";

const MessageInput = () => {
  const [text, setText] = useState("");
  const sendMessage = useChatStore((state) => state.sendMessage);
  const { colorMode } = useColorMode();

  const handleSend = () => {
    if (text.trim()) {
      sendMessage(text);
      setText("");
    }
  };

  return (
    <HStack sx={messageInputContainerStyles(colorMode)}>
      <IconButton
        aria-label="拍照"
        icon={<FiCamera />}
        variant="messageAction"
        colorScheme="gray"
      />
      <Input
        placeholder="輸入訊息..."
        value={text}
        onChange={(e) => setText(e.target.value)}
        onKeyDown={(e) => e.key === "Enter" && handleSend()}
        sx={messageInputStyles(colorMode)}
        flex={1}
      />
      <IconButton
        aria-label="發送"
        icon={<FiSend />}
        variant="messageAction"
        colorScheme="brand"
        onClick={handleSend}
      />
    </HStack>
  );
};

export default MessageInput;
