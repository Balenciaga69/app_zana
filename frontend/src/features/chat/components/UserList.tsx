// 使用者列表元件（可依需求擴充）
import { Box, Text } from "@chakra-ui/react";
import { useChatStore } from "../store/chatStore";

const UserList = () => {
  const users = useChatStore((state) => state.users);
  return (
    <Box mb={2}>
      <Text fontSize="sm" color="gray.500">在線使用者：{users.length}</Text>
      <Box display="flex" gap={2} flexWrap="wrap">
        {users.map((user, idx) => (
          <Text key={idx} fontSize="xs" bg="gray.200" px={2} borderRadius="md">{user}</Text>
        ))}
      </Box>
    </Box>
  );
};

export default UserList;
