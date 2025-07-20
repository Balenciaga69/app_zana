// 訊息列表元件
import { Box, Text, VStack } from "@chakra-ui/react";
import { useChatStore } from "../store/chatStore";

const MessageList = () => {
  const messages = useChatStore((state) => state.messages);
  return (
    <VStack align="stretch" spacing={2} h="300px" overflowY="auto" mb={2}>
      {messages.map((msg, idx) => (
        <Box key={idx} bg="gray.100" p={2} borderRadius="md">
          <Text fontWeight="bold">{msg.user}</Text>
          <Text>{msg.text}</Text>
        </Box>
      ))}
    </VStack>
  );
};

export default MessageList;
