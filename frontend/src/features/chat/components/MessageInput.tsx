// 訊息輸入元件
import { HStack, Input, Button } from "@chakra-ui/react";
import { useState } from "react";
import { useChatStore } from "../store/chatStore";

const MessageInput = () => {
  const [text, setText] = useState("");
  const sendMessage = useChatStore((state) => state.sendMessage);

  const handleSend = () => {
    if (text.trim()) {
      sendMessage(text);
      setText("");
    }
  };

  return (
    <HStack>
      <Input
        placeholder="輸入訊息..."
        value={text}
        onChange={(e) => setText(e.target.value)}
        onKeyDown={(e) => e.key === "Enter" && handleSend()}
      />
      <Button colorScheme="blue" onClick={handleSend}>
        發送
      </Button>
    </HStack>
  );
};

export default MessageInput;
