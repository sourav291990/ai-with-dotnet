## How to run ollama in local using docker image

docker run -d -v ollama:/root/.ollama -p 11434:11434 --name ollama ollama/ollama


docker exec -it ollama ollama run all-minilm
