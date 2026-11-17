/*
Попробовал написать код для задачи по TDD:
	Нужно было поменять существующий код клиента "системной шины" чтобы он в случае ошибки на Ack сообщения, повторял полностью цикл обработки сообщения, а не только Ack.
    Задача была сформулирована разговорным языком, без описания сценариев и граничных случаев. Поэтому я сначала для себя составил описание сценариев и уточнил граничные случаи.

	Сценарии:
		- happy-path - сообщение принято без ошибки, 
		- возникла ошибка во время receive
		- возникла ошибка во время ack и произошел ретрай всей итерации.

	Условия:
    	- код не должен использовать значения retryNumber и retryDelay

    Согласно сценариям я описал тест кейсы. Во время работы заметил что получаемая реализация следует за тестами и если тесты не достаточно полно описывают систему, то возможны пробелы и в реализации.
	Также получаемое решение выглядит неоптимизированным и требует рефакторинга.

	TDD подразумевает три фазы: "red", "green", "refactor", в итоге требуется провестия несколько итераций рефакторинга для завершения задачи.
	В условиях неопределенных требований, или на этапе их уточнения такой вариант работы выглядит вполне удобным. И также сценарии в тестах позволяют писать код и сразу отлаживать на основе тестовых данных.

Попробовал также написать код, который не следует тестам, и тесты, которые не следуют коду, а вместо этого и код и тесты следует дизайну, логическим требованиям.

	Написать тест на то что ретрай происходит после первой же ошибки.
		
		[TestMethod]
		public async Task AckAsync_ThrowsErrorOnFirstFailure()
		{
			var consumer = new FakeConsumer();
			var error = new ChannelException(ChannelErrorType.Unreachable, "error");
			consumer.OnAck = (_, _) => throw error;

			var thrown = await Assert.ThrowsExactlyAsync<ChannelException>(
				async () => await NewChannel(consumer, 1).CommitAsync(NewMessage(), CancellationToken.None)
			);

			Assert.AreSame(error, thrown);
			Assert.AreEqual(1, consumer.AckCalls);
		}

	Удалить использование полей в коде.
		- Удалил условие для ретрая во время Ack

		public async Task CommitAsync(Message message, CancellationToken cancellationToken)
		{
			var queueName = _consumer.Destination.Name;
			var messageId = message.Id;

			try {
				_logger.LogDebug("Acknowledging message {MessageId} from queue '{Queue}' (attempt {Attempt})", message.Id, queueName, attempt);

				await _consumer.AckAsync(message.Id, cancellationToken);

				_logger.LogDebug("Acknowledged message {MessageId} from queue '{Queue}' in {Elapsed}ms", messageId, queueName, sw.ElapsedMilliseconds);

				break;
			}
			catch (SystemException) {
				throw;
			}
			catch (Exception ex) {
				_logger.Log(LogLevel.Error, ex, "Problem acknowledging message {MessageId} from queue '{Queue}'", message.Id, queueName);

				// Удалил цикл в котором исполнялся код и условие для ретрая
				// if (attempt >= _retryAttempts) {
				//    throw;
				// }
				throw;
			}
		}
		
	Проверить работу прогоном тестов.  

	Можно сначала написать код и потом добавить тесты описывающие спецификацию. Тут есть соблазн подогнать тесты под код. Поэтому думаю что предпочтительнее сначала добавить тесты по дизайну и потом реализацию. 
	Выглядит как удобный способ работы при наличии четкого дизайна и логических требований с самого начала.

	Как вариант совмещения обоих подходов можно выполнять мини-итерации вида описание части спецификации по дизайну потом реализация по дизайну и проверка насколько тесты с реализацией совпадают. 
*/